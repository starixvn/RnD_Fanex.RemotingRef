namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class CapacityTests : IDisposable
    {
        private readonly LocalStorageOptions _storageOptions;

        private LocalStorage _storage;

        public CapacityTests()
        {
            var clock = new TestClock();

            _storageOptions = new LocalStorageOptions
            {
                NumberOfEntriesLimit = 1,
                ExpirationScanFrequency = TimeSpan.Zero,
                CompactionPercentage = 0.5
            };

            _storage = new LocalStorage(_storageOptions, clock);
        }

        [Fact]
        public void NegativeNumberOfEntriesLimitThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var negativeNumberOfEntriesLimit = -1;

            // Act.
            Exception exception = Record.Exception(() => new LocalStorageOptions
            {
                NumberOfEntriesLimit = negativeNumberOfEntriesLimit
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void NewEntryIsDiscardedIfItExceedsCapacity()
        {
            // Arrange.
            var entry = CreateCacheEntry("myStupidKey");

            _storage.Add(entry);

            // Act.
            var newKey = "myOtherStupidEntry";
            var newEntry = CreateCacheEntry("myOtherStupidEntry");

            _storage.Add(newEntry);

            // Assert.
            var found = _storage.TryGet(newKey, out _);

            Assert.False(found);
        }

        [Fact]
        public async Task ExceedsCapacityCompactsTheStorage()
        {
            // Arrange.
            var firstEntry = CreateCacheEntry("myFirstStupidKey");
            var secondEntry = CreateCacheEntry("mySecondStupidKey");
            var semaphore = new SemaphoreSlim(0, 1);

            _storageOptions.CompactionPercentage = 1;
            firstEntry.PostEvictionCallback = () =>
            {
                semaphore.Release();
            };

            // Act.
            _storage.Add(firstEntry);
            _storage.Add(secondEntry);

            // Assert.
            Assert.True(await semaphore.WaitAsync(TimeSpan.FromSeconds(30)));
            Assert.Equal(0, _storage.Count);
        }

        private CacheEntry CreateCacheEntry(string expectedKey = null)
        {
            var keySet = new KeySet
            {
                ["AnotherStupidDuplication"] = "StupidValue"
            };
            var key = expectedKey ?? keySet.BuildKey();

            return _storage.CreateEntry(key, keySet, TranslationSet.FromDictionary(new Dictionary<string, string>()));
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}