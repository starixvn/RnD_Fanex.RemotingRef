namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Threading;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class TimeExpirationTests : IDisposable
    {
        private readonly TestClock _clock;

        private LocalStorage _storage;

        public TimeExpirationTests()
        {
            var storageOptions = new LocalStorageOptions
            {
                SlidingTimeout = TimeSpan.FromSeconds(10)
            };

            _clock = new TestClock();
            _storage = new LocalStorage(storageOptions, _clock);
        }

        [Fact]
        public void NegativeSlidingTimeoutThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var negativeSlidingTimeout = TimeSpan.FromSeconds(-1);

            // Act.
            var exception = Record.Exception(() => new LocalStorageOptions
            {
                SlidingTimeout = negativeSlidingTimeout
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void ZeroSlidingTimeoutThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var zeroSlidingTimeout = TimeSpan.Zero;

            // Act.
            var exception = Record.Exception(() => new LocalStorageOptions
            {
                SlidingTimeout = zeroSlidingTimeout
            });

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void KeysExpireInBackground()
        {
            // Arrange.
            var (key, entry) = _storage.CreateCacheEntry();
            var callbackInvoked = new ManualResetEventSlim(false);

            entry.PostEvictionCallback = callbackInvoked.Set;
            _storage.Add(entry);

            // Act.
            // Background expiration checks are triggered when TryGet is called no matter what the key is.
            const double numberOfMunitesPassedToMakeKeyBeExpired = 20;

            var anyOtherKey = LocalStorageExtensions.CreateKey();

            _clock.AddMinutes(numberOfMunitesPassedToMakeKeyBeExpired);
            _storage.TryGet(anyOtherKey, out _);

            // Assert.
            var waitingTime = TimeSpan.FromSeconds(30);

            Assert.True(callbackInvoked.Wait(waitingTime));
            Assert.False(_storage.TryGet(key, out _));
        }

        [Fact]
        public void KeyExpiresIfNotBeingTouched()
        {
            // Arrange.
            var (key, entry) = _storage.AddCacheEntry();
            bool found = _storage.TryGet(key, out CacheEntry addedEntry);

            Assert.True(found);
            Assert.Same(entry, addedEntry);

            // Act.
            const double numberOfMunitesPassedToMakeKeyBeExpired = 1;

            _clock.AddMinutes(numberOfMunitesPassedToMakeKeyBeExpired);

            // Assert.
            found = _storage.TryGet(key, out addedEntry);

            Assert.False(found);
            Assert.Null(addedEntry);
        }

        [Fact]
        public void KeyIsRenewedOnceBeingTouched()
        {
            // Arrange.
            var (key, entry) = _storage.AddCacheEntry();

            // Act - touched after 5 seconds.
            const double timePassedForFirstTouch = 5;

            _clock.AddSeconds(timePassedForFirstTouch);
            _storage.TryGet(key, out _);

            // Assert - after 9 seconds from last touch.
            const double timePassedForLastTouch = 9;

            _clock.AddSeconds(timePassedForLastTouch);

            var found = _storage.TryGet(key, out CacheEntry addedEntry);

            Assert.True(found);
            Assert.Same(entry, addedEntry);
        }

        [Fact]
        public void AddExpiredCacheEntryMatchingWithExistingKeyRemovesTheExistingCacheEntry()
        {
            // Arrange.
            var (key, firstEntry) = _storage.AddCacheEntry();

            // Act.
            const double numberOfSecondsPassedToMakeKeysBeExpired = 30;

            var secondEntry = _storage.CloneCacheEntry(firstEntry);

            _clock.AddSeconds(numberOfSecondsPassedToMakeKeysBeExpired);
            _storage.Add(secondEntry);

            // Assert.
            Assert.False(_storage.TryGet(key, out _));
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}