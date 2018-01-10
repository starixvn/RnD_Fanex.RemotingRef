namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Tests.Mocks;
    using Fanex.RemotingRef.TranslationStorage;
    using Xunit;

    public sealed class SyncingTranslationSetsTests : IDisposable
    {
        private readonly IRefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly TestClock _clock;
        private readonly ManualResetEventSlim _callbackInvoked;
        private readonly TimeSpan _waitingTime;
        private readonly TimeSpan _timeToTriggerRenewal;

        private LocalStorage _storage;

        public SyncingTranslationSetsTests()
        {
            _options = new RefDataTypeHandlerOptions();
            _clock = new TestClock();
            _storage = new LocalStorage(_options, _clock);
            _handler = new RefDataTypeHandler(_options, _storage);

            _options.RenewalTime = TimeSpan.FromSeconds(1);
            _options.SyncBatchSize = 2;

            _callbackInvoked = new ManualResetEventSlim(false);
            _waitingTime = TimeSpan.FromSeconds(30);
            _timeToTriggerRenewal = _options.RenewalTime + TimeSpan.FromSeconds(1);
        }

        [Fact]
        public void NegativeRenewalTimeThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var negativeRenewalTime = TimeSpan.FromSeconds(-1);

            // Act.
            Exception exception = Record.Exception(() => _options.RenewalTime = negativeRenewalTime);

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void RetrievedEntryMetRenewalTimeTriggersSyncProcess()
        {
            // Arrange.
            var existingKeySet = SetupStorageWithOnlyOneEntry();

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            _handler.Get(existingKeySet, null);

            // Assert.
            Assert.True(_callbackInvoked.Wait(_waitingTime));
        }

        [Fact]
        public void RenewalPrioritizesLatestAccessEntries()
        {
            // Arrange.
            const int expectedEntries = 4;
            const int assumedCreationInterval = 2;

            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            var firsKeySet = createdKeySets[0];

            Assert.True(_callbackInvoked.Wait(_waitingTime));
            Assert.DoesNotContain(firsKeySet, sendingKeySets);
        }

        [Fact]
        public void RenewalCollectsAtMostSyncBatchSizePlusOne()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 2;

            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(_callbackInvoked.Wait(_waitingTime));
            Assert.Equal(_options.SyncBatchSize + 1, sendingKeySets.Count);
        }

        [Fact]
        public void RenewalDoesNotCollectEntriesUnmetRenewalTime()
        {
            // Arrange.
            const int expectedEntries = 4;
            const int assumedCreationInterval = 1;

            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var firstKeySet = createdKeySets[0];

            _handler.Get(firstKeySet, null);

            // Assert.
            _storage.TryGet(createdKeySets[expectedEntries - 1].BuildKey(), out CacheEntry unmetRenewalEntry);

            Assert.True(_callbackInvoked.Wait(_waitingTime));
            Assert.False(unmetRenewalEntry.NeedToRenew(_clock.UtcNow));
            Assert.DoesNotContain(unmetRenewalEntry.OriginalKeySet, sendingKeySets);
        }

        [Fact]
        public void AllEntriesMetRenewalTimeAreCollectedIfSyncBatchSizeIsNotSatisfied()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 2;

            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };
            _options.SyncBatchSize = 10;

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(_callbackInvoked.Wait(_waitingTime));
            Assert.Equal(expectedEntries, sendingKeySets.Count);
        }

        [Fact]
        public void EmptyResponseLetsRenewingLocksBeReleased()
        {
            // Arrange.
            var createdKeySet = SetupStorageWithOnlyOneEntry();
            var key = createdKeySet.BuildKey();
            var emptyValue = new Dictionary<string, TranslationSet>();

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                _callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(emptyValue);
            };
            _storage.TryGet(key, out CacheEntry oldEntry);

            // Act.
            _handler.Get(createdKeySet, null);

            // Assert.
            Assert.True(_callbackInvoked.Wait(_waitingTime));

            _storage.TryGet(key, out CacheEntry currentEntry);

            Assert.Same(oldEntry.Value, currentEntry.Value);
        }

        [Fact]
        public async Task NonemptyResponseUpdatesDataInStorage()
        {
            // Arrange.
            var createdKeySet = SetupStorageWithOnlyOneEntry();
            var key = createdKeySet.BuildKey();
            var newValue = TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = "NewStupidText"
            });
            var syncedData = new Dictionary<string, TranslationSet>
            {
                [key] = newValue
            };
            var semaphore = new SemaphoreSlim(0, 1);

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                semaphore.Release();

                return Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);
            };

            // Act.
            _handler.Get(createdKeySet, null);

            // Assert.
            Assert.True(await semaphore.WaitAsync(_waitingTime));

            _storage.TryGet(key, out CacheEntry currentEntry);

            Assert.Same(newValue, currentEntry.Value);
        }

        [Fact]
        public async Task LockedEntryIsReleasedIfNotInResponse()
        {
            // Arrange.
            const int expectedEntries = 2;
            const int assumedCreationInterval = 2;

            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            var lastKeySet = createdKeySets[expectedEntries - 1];
            var key = lastKeySet.BuildKey();
            var newValue = TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = "NewStupidText"
            });
            var syncedData = new Dictionary<string, TranslationSet>
            {
                [key] = newValue
            };
            var semaphore = new SemaphoreSlim(0, 1);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                semaphore.Release();

                return Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);
            };

            // Act.
            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(await semaphore.WaitAsync(_waitingTime));

            _storage.TryGet(createdKeySets[0].BuildKey(), out CacheEntry entry);

            Assert.True(entry.NeedToRenew(_clock.UtcNow));
        }

        private KeySet[] SetupStorageWithBatchOfData(int expectedLength, int creationInterval)
        {
            var createdEntries = new CacheEntry[expectedLength];

            for (int index = 0; index < expectedLength; index++)
            {
                var keySet = new KeySet
                {
                    ["Key" + index] = "Key" + index
                };
                var value = TranslationSet.FromDictionary(new Dictionary<string, string>
                {
                    [RefDataTypeHandlerOptions.DefaultLanguage] = "StupidText"
                });

                createdEntries[index] = _storage.CreateEntry(keySet.BuildKey(), keySet, value);
                _storage.Add(createdEntries[index]);
                _clock.Add(TimeSpan.FromSeconds(creationInterval));
            }

            return createdEntries.Select(enrty => enrty.OriginalKeySet).ToArray();
        }

        private KeySet SetupStorageWithOnlyOneEntry()
        {
            var keySet = new KeySet
            {
                ["StupidKey"] = "StupidValue"
            };
            var key = keySet.BuildKey();
            var value = new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = "StupidText"
            };
            var entry = _storage.CreateEntry(key, keySet, TranslationSet.FromDictionary(value));

            _storage.Add(entry);

            return keySet;
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}