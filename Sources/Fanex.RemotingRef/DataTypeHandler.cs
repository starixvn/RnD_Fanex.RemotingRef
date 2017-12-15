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
            _localStorage = new MemoryStorage(options);
        }

        public DataTypeHandlerOptions Options { get; }

        public string Get(ExtraDataSet parameters, string languageCode, string defaultValue)
        {
            string result = null;
            string key = Options.KeyFactory(parameters);

            if (_localStorage.TryGet(key, out CacheEntry cachedEntry) && cachedEntry != null)
            {
                result = cachedEntry.Value[languageCode] ?? cachedEntry.Value[DataTypeHandlerOptions.DefaultLanguage];

                if (cachedEntry.NeedToRenew(_localStorage.Clock.UtcNow))
                {
                    TriggerSyncCachedData(cachedEntry);
                }
            }
            else
            {
                TranslationSet newTranslationSet = Options.ValueFactory(parameters);

                if (newTranslationSet != null)
                {
                    CacheEntry newEntry = _localStorage.CreateEntry(key, newTranslationSet, parameters);

                    _localStorage.Add(key, newEntry);
                    result = newTranslationSet[languageCode] ?? newTranslationSet[DataTypeHandlerOptions.DefaultLanguage];
                }
            }

            return result ?? defaultValue;
        }

        private void TriggerSyncCachedData(CacheEntry rootEntry)
            => ThreadPool.QueueUserWorkItem(SyncCachedData, rootEntry);

        private void SyncCachedData(object state)
        {
            if (state is CacheEntry rootEntry)
            {
                IList<CacheEntry> batch = _localStorage.FilterOutOfDateEntries(Options.UpdatingBatchSize);

                batch.Add(rootEntry);

                IList<ExtraDataSet> parameterSets = batch.Select(item => item.ExtraData).ToList();
                IDictionary<string, TranslationSet> results = Options.SyncDataHandler(parameterSets);

                if (results != null && results.Count > 0)
                {
                    SyncDataInStorage(batch, results);
                }
            }
        }

        private void SyncDataInStorage(IList<CacheEntry> existingEntries, IDictionary<string, TranslationSet> newDataMaps)
        {
            foreach (CacheEntry entry in existingEntries)
            {
                string key = entry.Key;

                if (newDataMaps.ContainsKey(key))
                {
                    CacheEntry updatedEntry = _localStorage.CreateEntry(key, newDataMaps[key], entry.ExtraData);

                    _localStorage.TryUpdate(entry, updatedEntry);
                }
            }
        }
    }
}