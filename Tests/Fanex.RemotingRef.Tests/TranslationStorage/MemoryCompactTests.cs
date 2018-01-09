namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class MemoryCompactTests : IDisposable
    {
        private readonly TestClock _clock;

        private LocalStorage _storage;

        public MemoryCompactTests()
        {
            var storageOptions = new LocalStorageOptions();

            _clock = new TestClock();
            _storage = new LocalStorage(storageOptions, _clock);
        }

        [Fact]
        public void NegativeCompactionPercentageThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var negativeCompactionPercentage = -0.1;

            // Act.
            Exception exception = Record.Exception(() => new LocalStorageOptions
            {
                CompactionPercentage = negativeCompactionPercentage
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void GreaterThanOneCompactionPercentageThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var outOfRangeCompactionPercentage = 1.1;

            // Act.
            Exception exception = Record.Exception(() => new LocalStorageOptions
            {
                CompactionPercentage = outOfRangeCompactionPercentage
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void Compact100PercentClearsAlls()
        {
            // Arrange.
            InitializeCacheData();

            // Act.
            _storage.Compact(1.0);

            // Assert.
            Assert.Equal(0, _storage.Count);
        }

        [Fact]
        public void CompactPrioritizesLastAccess()
        {
            // Arrange.
            var cachedKeys = InitializeCacheData();

            // Act.
            _storage.Compact(0.90);

            // Assert.
            bool found = _storage.TryGet(cachedKeys.Last(), out _);

            Assert.Equal(1, _storage.Count);
            Assert.True(found);
        }

        [Fact]
        public void CompactOnMemoryPressureTriggersDoMemoryPreassureCollection()
        {
            // Arrange.
            var storageOptions = new LocalStorageOptions
            {
                CompactOnMemoryPressure = true,
                CompactionPercentage = 0.5
            };

            _storage = new LocalStorage(storageOptions, _clock);

            var cachedKeys = InitializeCacheData();

            // Act.
            TriggerGargabeCollectionInItsOwnScope();

            // Assert.
            Assert.True(cachedKeys.Length > _storage.Count);
        }

        [Fact]
        public void DoMemoryPreassureCollectionTriesToSatisfyRemovalCountTarget()
        {
            // Arrange.
            const double compactPercentage = 0.5;
            var storageOptions = new LocalStorageOptions
            {
                CompactOnMemoryPressure = true,
                CompactionPercentage = compactPercentage
            };

            _storage = new LocalStorage(storageOptions, _clock);

            var cachedKeys = InitializeCacheData();

            // Act.
            TriggerGargabeCollectionInItsOwnScope();

            // Assert.
            int expectedCount = cachedKeys.Length - (int)(compactPercentage * cachedKeys.Length);

            Assert.True(expectedCount >= _storage.Count);
        }

#pragma warning disable S1215 // "GC.Collect" should not be called

        private static void TriggerGargabeCollectionInItsOwnScope()
        {
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

#pragma warning restore S1215 // "GC.Collect" should not be called

        private string[] InitializeCacheData()
        {
            int expectedLength = 4;
            var createdEntries = new CacheEntry[expectedLength];

            for (int index = 0; index < expectedLength; index++)
            {
                var keySet = new KeySet
                {
                    ["Key" + index] = "Key" + index
                };
                var value = TranslationSet.FromDictionary(new Dictionary<string, string>
                {
                    ["Value" + index] = "Value" + index
                });

                createdEntries[index] = _storage.CreateEntry(keySet.BuildKey(), keySet, value);
                _storage.Add(createdEntries[index]);
                _clock.Add(TimeSpan.FromSeconds(1));
            }

            return createdEntries.Select(enrty => enrty.Key).ToArray();
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}