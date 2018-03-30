namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Collections.Generic;
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
            Exception exception = Record.Exception(() => new LocalStorageOptions
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
            Exception exception = Record.Exception(() => new LocalStorageOptions
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
            var key = "myKey";
            var entry = CreateCacheEntry(key);
            var callbackInvoked = new ManualResetEventSlim(false);

            entry.PostEvictionCallback = callbackInvoked.Set;
            _storage.Add(entry);

            // Act.
            // Background expiration checks are triggered when TryGet is called no matter what the key is.
            _clock.Add(TimeSpan.FromMinutes(20));
            _storage.TryGet("otherKey", out _);

            // Assert.
            Assert.True(callbackInvoked.Wait(TimeSpan.FromSeconds(30)));
            Assert.False(_storage.TryGet(key, out _));
        }

        [Fact]
        public void KeyExpiresIfNotBeingTouched()
        {
            // Arrange.
            var key = "myKey";
            var entry = CreateCacheEntry(key);
            bool found;

            _storage.Add(entry);
            found = _storage.TryGet(key, out CacheEntry addedEntry);

            Assert.True(found);
            Assert.Same(entry, addedEntry);

            // Assert.
            _clock.Add(TimeSpan.FromMinutes(1));

            found = _storage.TryGet(key, out addedEntry);

            Assert.False(found);
            Assert.Null(addedEntry);
        }

        [Fact]
        public void KeyIsRenewedOnceBeingTouched()
        {
            // Arrange.
            var key = "myKey";
            var entry = CreateCacheEntry(key);

            _storage.Add(entry);

            // Act - touched after 5 seconds.
            _clock.Add(TimeSpan.FromSeconds(5));
            _storage.TryGet(key, out _);

            // Assert - after 9 seconds from last touch.
            _clock.Add(TimeSpan.FromSeconds(9));

            var found = _storage.TryGet(key, out CacheEntry addedEntry);

            Assert.True(found);
            Assert.Same(entry, addedEntry);
        }

        [Fact]
        public void AddExpiredCacheEntryMatchingWithExistingKeyRemovesTheExistingCacheEntry()
        {
            // Arrange.
            var key = "uniqueKey";
            var cacheEntry = CreateCacheEntry(key);
            var newCacheEntry = CreateCacheEntry(key);

            // Act.
            _storage.Add(cacheEntry);
            _clock.Add(TimeSpan.FromSeconds(30));
            _storage.Add(newCacheEntry);

            // Assert.
            Assert.False(_storage.TryGet(key, out _));
        }

        private CacheEntry CreateCacheEntry(string expectedKey)
        {
            var keySet = new KeySet
            {
                ["StupidKey"] = "StupidValue"
            };
            var key = expectedKey;

            return _storage.CreateEntry(key, keySet, TranslationSet.FromDictionary(new Dictionary<string, string>()));
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}