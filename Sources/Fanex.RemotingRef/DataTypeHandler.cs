namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using TranslationStorage;

    public class DataTypeHandler : IDataTypeHandler
    {
        private readonly IStorage _localStorage;

        public DataTypeHandler(DataTypeHandlerOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _localStorage = new LocalStorage(options);
        }

        internal DataTypeHandler(DataTypeHandlerOptions options, IStorage storage)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _localStorage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public DataTypeHandlerOptions Options { get; }

        public object Get(KeySet keySet, string languageCode)
        {
            object result = null;
            string key = keySet.ToHashKey();

            if (_localStorage.TryGet(key, out CacheEntry cachedEntry))
            {
                result = cachedEntry.Value[languageCode] ?? cachedEntry.Value[DataTypeHandlerOptions.DefaultLanguage];

                if (cachedEntry.NeedToRenew(_localStorage.Clock.UtcNow))
                {
                    TriggerSyncDataProcess(cachedEntry);
                }
            }
            else
            {
                TranslationSet newTranslationSet = Options.ValueFactory(keySet);

                if (newTranslationSet != null)
                {
                    CacheEntry newEntry = _localStorage.CreateEntry(key, keySet, newTranslationSet);

                    _localStorage.Add(newEntry);
                    result = newTranslationSet[languageCode] ?? newTranslationSet[DataTypeHandlerOptions.DefaultLanguage];
                }
            }

            return result;
        }

        private void TriggerSyncDataProcess(CacheEntry rootEntry)
            => ThreadPool.QueueUserWorkItem(SyncCachedData, rootEntry);

        private void SyncCachedData(object state)
        {
            if (state is CacheEntry rootEntry)
            {
                IList<CacheEntry> batch = _localStorage.FilterOutOfDateEntries(Options.UpdatingBatchSize);

                batch.Add(rootEntry);

                IList<KeySet> parameterSets = batch.Select(item => item.OriginalKeySet).ToList();
                IDictionary<string, TranslationSet> results = Options.SyncDataHandler(parameterSets);

                if (results != null && results.Count > 0)
                {
                    SyncDataInStorage(batch, results);
                }
                else
                {
                    ReleaseRenewingLocks(batch);
                }
            }
        }

        private void SyncDataInStorage(IList<CacheEntry> existingEntries, IDictionary<string, TranslationSet> newEntryMaps)
        {
            foreach (CacheEntry entry in existingEntries)
            {
                string key = entry.Key;

                if (newEntryMaps.ContainsKey(key))
                {
                    CacheEntry updatedEntry = _localStorage.CreateEntry(key, entry.OriginalKeySet, newEntryMaps[key]);

                    _localStorage.TryUpdate(updatedEntry, entry);
                }
                else
                {
                    entry.ReleaseRenewingLock();
                }
            }
        }

        private static void ReleaseRenewingLocks(IList<CacheEntry> lockedEntries)
        {
            foreach (CacheEntry lockedEntry in lockedEntries)
            {
                lockedEntry.ReleaseRenewingLock();
            }
        }
    }
}