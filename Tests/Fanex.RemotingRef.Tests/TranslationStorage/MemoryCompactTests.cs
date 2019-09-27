namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Linq;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class MemoryCompactTests : IDisposable
    {
        private const double CompactPercentage = 0.5;

        private readonly TestClock _clock;

        private LocalStorage _storage;

        public MemoryCompactTests()
        {
            var storageOptions = new LocalStorageOptions
            {
                CompactOnMemoryPressure = true,
                CompactionPercentage = CompactPercentage
            };

            _clock = new TestClock();
            _storage = new LocalStorage(storageOptions, _clock);
        }

        [Fact]
        public void NegativeCompactionPercentageThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            const double negativeCompactionPercentage = -0.1;

            // Act.
            var exception = Record.Exception(() => new LocalStorageOptions
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
            const double outOfRangeCompactionPercentage = 1.1;

            // Act.
            var exception = Record.Exception(() => new LocalStorageOptions
            {
                CompactionPercentage = outOfRangeCompactionPercentage
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void Compact100PercentClearsAllEntries()
        {
            // Arrange.
            InitializeCacheData();

            // Act.
            const double fullCompaction = 1;

            _storage.Compact(fullCompaction);

            // Assert.
            Assert.Equal(0, _storage.Count);
        }

        [Fact]
        public void CompactPrioritizesLastAccess()
        {
            // Arrange.
            var cachedKeys = InitializeCacheData();

            // Act.
            const double highCompaction = 0.9;

            _storage.Compact(highCompaction);

            // Assert.
            bool found = _storage.TryGet(cachedKeys.Last(), out _);

            Assert.Equal(1, _storage.Count);
            Assert.True(found);
        }

        [Fact]
        public void CompactOnMemoryPressureTriggersDoMemoryPreassureCollection()
        {
            // Arrange.
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
            var cachedKeys = InitializeCacheData();

            // Act.
            TriggerGargabeCollectionInItsOwnScope();

            // Assert.
            var expectedCount = cachedKeys.Length - (int)(CompactPercentage * cachedKeys.Length);

            Assert.True(expectedCount >= _storage.Count);
        }

#pragma warning disable S1215 // "GC.Collect" should not be called

        private static void TriggerGargabeCollectionInItsOwnScope()
        {
            const int oldestGenerationToBeCollected = 2;

            GC.Collect(oldestGenerationToBeCollected, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

#pragma warning restore S1215 // "GC.Collect" should not be called

        private string[] InitializeCacheData()
        {
            const int expectedLength = 4;

            var createdKeys = new string[expectedLength];
            var creationInterval = TimeSpan.FromSeconds(1);

            for (var index = 0; index < expectedLength; index++)
            {
                var (key, entry) = _storage.AddCacheEntry();

                createdKeys[index] = key;
                _clock.Add(creationInterval);
            }

            return createdKeys;
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}