namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class StorageGetAddTests : IDisposable
    {
        private LocalStorage _storage;

        public StorageGetAddTests()
        {
            var clock = new TestClock();
            var storageOptions = new LocalStorageOptions
            {
                SlidingTimeout = TimeSpan.FromSeconds(5)
            };

            _storage = new LocalStorage(storageOptions, clock);
        }

        [Fact]
        public void ArgumentExceptionIsThrownIfTCreateEntryTakesNullOrEmtyAsKey()
        {
            // Arrange.
            var key = string.Empty;
            var keySet = new KeySet();
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            // Act.
            Exception exception = Record.Exception(() => _storage.CreateEntry(key, keySet, value));

            // Assert.
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ArgumentExceptionIsThrownIfTCreateEntryTakesNullAsKeySet()
        {
            // Arrange.
            var key = "anyKey";
            KeySet keySet = null;
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            // Act.
            Exception exception = Record.Exception(() => _storage.CreateEntry(key, keySet, value));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ArgumentExceptionIsThrownIfTryGetTakesNullOrEmtyAsKey()
        {
            // Arrange.
            var key = string.Empty;

            // Act.
            Exception exception = Record.Exception(() => _storage.TryGet(key, out _));

            // Assert.
            Assert.IsType<ArgumentException>(exception);
        }

#pragma warning disable S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods

        [Fact]
        public void ObjectDisposedExceptionIsThrownIfTryGetOrAddIsCalledAfterDisposingStorage()
        {
            // Arrange.
            var key = "someKey";

            _storage.Dispose();

            // Act.
            Exception exception = Record.Exception(() => _storage.TryGet(key, out _));

            // Assert.
            Assert.IsType<ObjectDisposedException>(exception);
        }

#pragma warning restore S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods

        [Fact]
        public void TryGetMissingKeyReturnsFalse()
        {
            // Arrange.
            var key = "missingKey";

            // Act.
            bool found = _storage.TryGet(key, out _);

            // Assert.
            Assert.False(found);
        }

        [Fact]
        public void TryGetMissingKeyReturnsNullAsOutput()
        {
            // Arrange.
            var key = "missingKey";

            // Act.
            _storage.TryGet(key, out CacheEntry result);

            // Assert.
            Assert.Null(result);
        }

        [Fact]
        public void TryGetCanPickObjectStoredByAdd()
        {
            // Arrange.
            var cacheEntry = CreateCacheEntry();
            var key = cacheEntry.Key;

            _storage.Add(cacheEntry);

            // Act.
            bool found = _storage.TryGet(key, out CacheEntry result);

            // Assert.
            Assert.True(found);
            Assert.Same(cacheEntry, result);
        }

        [Fact]
        public void AddAndTryGetDoNotWorkWithCaseInsensitiveKeys()
        {
            // Arrange.
            var key = "uniqueKey";
            var cacheEntry = CreateCacheEntry(key);

            // Act.
            _storage.Add(cacheEntry);

            bool found = _storage.TryGet(key.ToUpperInvariant(), out CacheEntry result);

            // Assert.
            Assert.False(found);
            Assert.Null(result);
        }

        [Fact]
        public void AddAlwaysOverwritesExistingCacheEntry()
        {
            // Arrange.
            var key = "uniqueKey";
            var cacheEntry = CreateCacheEntry(key);
            var newCacheEntry = CreateCacheEntry(key);

            // Act.
            _storage.Add(cacheEntry);
            _storage.Add(newCacheEntry);

            // Assert.
            _storage.TryGet(key, out CacheEntry result);
            Assert.Same(newCacheEntry, result);
        }

        [Fact]
        public void TryGetAndAddAreThreadSafe()
        {
            // Arrange.
            var key = "uniqueKey";
            var cacheEntry = CreateCacheEntry(key);
            var newCacheEntry = CreateCacheEntry(key);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act.
            var firstTask = Task.Run(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    _storage.Add(cacheEntry);
                }
            });

            var secondTask = Task.Run(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    _storage.Add(newCacheEntry);
                }
            });

            var thirdTask = Task.Run(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    _storage.TryGet(key, out _);
                }
            });

            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

            Task.WaitAll(firstTask, secondTask, thirdTask);
            cancellationTokenSource.Dispose();

            // Assert.
            Assert.Equal(TaskStatus.RanToCompletion, firstTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, secondTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, thirdTask.Status);
        }

        private CacheEntry CreateCacheEntry(string expectedKey = null)
        {
            var keySet = new KeySet
            {
                ["Key"] = "Value"
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