namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using AutoFixture;
    using Fanex.RemotingRef.TranslationStorage;

    internal static class LocalStorageExtensions
    {
        private static readonly Fixture Fixture = new Fixture();

        static LocalStorageExtensions()
        {
            Fixture.Customizations.Add(new RandomNumericSequenceGenerator(0, 10));
        }

        public static string CreateKey()
            => Fixture.Create<string>();

        public static (KeySet, string) CreateKeySet()
        {
            var key = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            var keySet = new KeySet
            {
                [key] = value
            };

            return (keySet, keySet.BuildKey());
        }

        public static (KeySet, string) CreateRandomKeySet()
        {
            var key = Fixture.Create<int>().ToString(CultureInfo.InvariantCulture);
            var value = key;
            var keySet = new KeySet
            {
                [key] = value
            };

            return (keySet, keySet.BuildKey());
        }

        public static void AddRandomCacheEntry(this LocalStorage storage)
        {
            var (keySet, key) = CreateRandomKeySet();
            var value = CreateTranslationSet();
            var entry = storage.CreateEntry(key, keySet, value);

            storage.Add(entry);
        }

        public static TranslationSet CreateTranslationSet()
        {
            var value = Fixture.Create<string>();

            return TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = value
            });
        }

        public static (string, CacheEntry) CreateCacheEntry(this LocalStorage storage)
        {
            var (keySet, key) = CreateKeySet();
            var value = CreateTranslationSet();
            var entry = storage.CreateEntry(key, keySet, value);

            return (key, entry);
        }

        public static (string, CacheEntry) AddCacheEntry(this LocalStorage storage)
        {
            var (key, entry) = CreateCacheEntry(storage);

            storage.Add(entry);

            return (key, entry);
        }

        public static CacheEntry CloneCacheEntry(this LocalStorage storage, CacheEntry originalEntry)
        {
            var value = CreateTranslationSet();

            return storage.CreateEntry(originalEntry.Key, originalEntry.OriginalKeySet, value);
        }
    }
}