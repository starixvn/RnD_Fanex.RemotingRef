namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.TranslationStorage;

    internal class RefDataTypeHandler : IRefDataTypeHandler
    {
        private readonly IStorage _localStorage;

        public RefDataTypeHandler(RefDataTypeHandlerOptions options, IStorage storage)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _localStorage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public RefDataTypeHandlerOptions Options { get; }

        public object Get(KeySet keySet, string languageCode)
        {
            object result = null;
            var key = keySet.BuildKey();

            if (_localStorage.TryGet(key, out CacheEntry cachedEntry))
            {
                result = GetTranslationInSpecificLanguage(cachedEntry.Value, languageCode);

                if (cachedEntry.NeedToRenew(_localStorage.Clock.UtcNow))
                {
                    TriggerSyncDataProcess(cachedEntry);
                }
            }
            else
            {
                var newTranslationSet = RetryGettingTranslationSet(keySet);

                if (newTranslationSet != null)
                {
                    var newEntry = _localStorage.CreateEntry(key, keySet, newTranslationSet);

                    _localStorage.Add(newEntry);
                    result = GetTranslationInSpecificLanguage(newEntry.Value, languageCode);
                }
            }

            return result;
        }

        private static object GetTranslationInSpecificLanguage(TranslationSet translations, string languageCode)
        {
            var retrievingLanguageCode = string.IsNullOrEmpty(languageCode) ? RefDataTypeHandlerOptions.DefaultLanguage : languageCode;

            if (translations.ContainsKey(retrievingLanguageCode))
            {
                return translations[retrievingLanguageCode];
            }

            if (translations.ContainsKey(RefDataTypeHandlerOptions.DefaultLanguage))
            {
                return translations[RefDataTypeHandlerOptions.DefaultLanguage];
            }

            return null;
        }

        private TranslationSet RetryGettingTranslationSet(KeySet keySet)
        {
            var retryCounter = 0;

            while (retryCounter < Options.NumberOfRetries)
            {
                retryCounter++;

                try
                {
                    var translationSet = Task.Run(() => Options.RequestHandler(keySet)).GetAwaiter().GetResult();

                    if (translationSet != null)
                    {
                        return translationSet;
                    }
                }
                catch
                {
                    if (retryCounter == Options.NumberOfRetries)
                    {
                        throw;
                    }
                }
            }

            return default;
        }

        private void TriggerSyncDataProcess(CacheEntry rootEntry)
            => Task.Run(() => SyncCachedDataAsync(rootEntry));

        internal async Task SyncCachedDataAsync(CacheEntry rootEntry)
        {
            var batch = _localStorage.FilterOutOfDateEntries(Options.SyncBatchSize);

            batch.Add(rootEntry);

            var parameterSets = batch.Select(item => item.OriginalKeySet).ToList();
            var results = await Options.SyncHandler(parameterSets).ConfigureAwait(false);

            if (results != null && results.Count > 0)
            {
                UpdateDataInStorage(batch, results);
            }
            else
            {
                ReleaseRenewingLocks(batch);
            }
        }

        private void UpdateDataInStorage(IList<CacheEntry> existingEntries, IDictionary<string, TranslationSet> newEntryMaps)
        {
            foreach (var entry in existingEntries)
            {
                var key = entry.Key;

                if (!_localStorage.IsExpired(entry) && newEntryMaps.ContainsKey(key))
                {
                    var newTranslationSet = newEntryMaps[key];

                    if (newTranslationSet != null)
                    {
                        var updatedEntry = _localStorage.CreateEntry(key, entry.OriginalKeySet, newTranslationSet);

                        _localStorage.Add(updatedEntry);
                    }
                }
                else
                {
                    entry.ReleaseRenewingLock();
                }
            }
        }

        private static void ReleaseRenewingLocks(IList<CacheEntry> lockedEntries)
        {
            foreach (var lockedEntry in lockedEntries)
            {
                lockedEntry.ReleaseRenewingLock();
            }
        }
    }
}