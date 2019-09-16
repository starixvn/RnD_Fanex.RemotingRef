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
        public void ArgumentNullExceptionIsThrownIfCreateEntryTakesNullAsKey()
        {
            // Arrange.
            string key = null;
            var keySet = new KeySet();
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            // Act.
            var exception = Record.Exception(() => _storage.CreateEntry(key, keySet, value));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ArgumentExceptionIsThrownIfCreateEntryTakesNullAsKeySet()
        {
            // Arrange.
            const string key = "anyKey";

            KeySet keySet = null;
            var value = TranslationSet.FromDictionary(new Dictionary<string, string>());

            // Act.
            var exception = Record.Exception(() => _storage.CreateEntry(key, keySet, value));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void ArgumentNullExceptionIsThrownIfTryGetTakesNullAsKey()
        {
            // Arrange.
            string key = null;

            // Act.
            var exception = Record.Exception(() => _storage.TryGet(key, out _));

            // Assert.
            Assert.IsType<ArgumentNullException>(exception);
        }

#pragma warning disable S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods

        [Fact]
        public void ObjectDisposedExceptionIsThrownIfTryGetOrAddIsCalledAfterDisposingStorage()
        {
            // Arrange.
            var key = LocalStorageExtensions.CreateKey();

            _storage.Dispose();

            // Act.
            var exception = Record.Exception(() => _storage.TryGet(key, out _));

            // Assert.
            Assert.IsType<ObjectDisposedException>(exception);
        }

#pragma warning restore S2952 // Classes should "Dispose" of members from the classes' own "Dispose" methods

        [Fact]
        public void TryGetMissingKeyReturnsFalse()
        {
            // Arrange.
            var missingKey = LocalStorageExtensions.CreateKey();

            // Act.
            var found = _storage.TryGet(missingKey, out _);

            // Assert.
            Assert.False(found);
        }

        [Fact]
        public void TryGetMissingKeyReturnsNullAsOutput()
        {
            // Arrange.
            var missingKey = LocalStorageExtensions.CreateKey();

            // Act.
            _storage.TryGet(missingKey, out CacheEntry result);

            // Assert.
            Assert.Null(result);
        }

        [Fact]
        public void TryGetCanPickObjectStoredByAdd()
        {
            // Arrange.
            var (key, entry) = _storage.AddCacheEntry();

            // Act.
            var found = _storage.TryGet(key, out CacheEntry result);

            // Assert.
            Assert.True(found);
            Assert.Same(entry, result);
        }

        [Fact]
        public void AddAndTryGetDoNotWorkWithCaseInsensitiveKeys()
        {
            // Arrange.
            var (key, entry) = _storage.CreateCacheEntry();

            // Act.
            _storage.Add(entry);

            var found = _storage.TryGet(key.ToUpperInvariant(), out CacheEntry result);

            // Assert.
            Assert.False(found);
            Assert.Null(result);
        }

        [Fact]
        public void AddAlwaysOverwritesExistingCacheEntry()
        {
            // Arrange.
            var (key, entry) = _storage.AddCacheEntry();
            var newEntry = _storage.CloneCacheEntry(entry);

            // Act.
            _storage.Add(newEntry);

            // Assert.
            _storage.TryGet(key, out CacheEntry result);
            Assert.Same(newEntry, result);
        }

        [Fact]
        public void TryGetAndAddOneSpecificKeyAreThreadSafe()
        {
            // Arrange.
            var (keySet, key) = LocalStorageExtensions.CreateKeySet();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act.
            var firstTask = Task.Run(() => AddOneSpecificKey(keySet, cancellationToken), cancellationToken);
            var secondTask = Task.Run(() => TryOneSpecificKey(key, cancellationToken), cancellationToken);
            var thirdTask = Task.Run(() => cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(50)), cancellationToken);

            Task.WaitAll(firstTask, secondTask, thirdTask);
            cancellationTokenSource.Dispose();

            // Assert.
            Assert.Equal(TaskStatus.RanToCompletion, firstTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, secondTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, thirdTask.Status);
        }

        private void AddOneSpecificKey(KeySet keySet, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var entry = _storage.CreateEntry(
                    keySet.BuildKey(),
                    keySet,
                    LocalStorageExtensions.CreateTranslationSet());

                _storage.Add(entry);
            }
        }

        private void TryOneSpecificKey(string key, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _storage.TryGet(key, out _);
            }
        }

        [Fact]
        public void TryGetAndAddRandomKeysAreThreadSafe()
        {
            // Arrange.
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act.
            var firstTask = Task.Run(() => AddRandomKey(cancellationToken), cancellationToken);
            var secondTask = Task.Run(() => AddRandomKey(cancellationToken), cancellationToken);
            var thirdTask = Task.Run(() => TryGetRandomKey(cancellationToken), cancellationToken);
            var forthTask = Task.Run(() => cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(50)), cancellationToken);

            Task.WaitAll(firstTask, secondTask, thirdTask, forthTask);
            cancellationTokenSource.Dispose();

            // Assert.
            Assert.Equal(TaskStatus.RanToCompletion, firstTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, secondTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, thirdTask.Status);
            Assert.Equal(TaskStatus.RanToCompletion, forthTask.Status);
        }

        private void AddRandomKey(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _storage.AddRandomCacheEntry();
            }
        }

        private void TryGetRandomKey(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var (_, key) = LocalStorageExtensions.CreateRandomKeySet();

                _storage.TryGet(key, out _);
            }
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}