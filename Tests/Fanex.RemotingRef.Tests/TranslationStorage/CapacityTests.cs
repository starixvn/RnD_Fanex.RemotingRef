namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
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
            var exception = Record.Exception(() => new LocalStorageOptions
            {
                NumberOfEntriesLimit = negativeNumberOfEntriesLimit
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void NewEntryIsStillAcceptedIfItExceedsCapacity()
        {
            // Arrange.
            var (_, entry) = _storage.AddCacheEntry();

            // Act.
            var (newKey, newEntry) = _storage.AddCacheEntry();

            // Assert.
            var found = _storage.TryGet(newKey, out _);

            Assert.True(found);
        }

        [Fact]
        public async Task ExceedsCapacityCompactsTheStorage()
        {
            // Arrange.
            var (_, firstEntry) = _storage.AddCacheEntry();
            var (_, secondEntry) = _storage.CreateCacheEntry();
            var semaphore = new SemaphoreSlim(0, 1);

            _storageOptions.CompactionPercentage = 1;
            secondEntry.PostEvictionCallback = () =>
            {
                semaphore.Release();
            };

            // Act.
            _storage.Add(secondEntry);

            // Assert.
            var waitingTime = TimeSpan.FromSeconds(30);

            Assert.True(await semaphore.WaitAsync(waitingTime));
            Assert.Equal(0, _storage.Count);
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}