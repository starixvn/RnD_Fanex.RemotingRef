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
        private readonly RefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly TestClock _clock;
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
            var callbackInvoked = new ManualResetEventSlim(false);
            var existingKeySet = SetupStorageWithOnlyOneEntry().OriginalKeySet;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            _handler.Get(existingKeySet, null);

            // Assert.
            Assert.True(callbackInvoked.Wait(_waitingTime));
        }

        [Fact]
        public async Task RenewalPrioritizesLatestAccessEntries()
        {
            // Arrange.
            const int expectedEntries = 4;
            const int assumedCreationInterval = 1;

            var createdEntries = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            var firstCreatedEntry = createdEntries[0];
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var randomEntry = createdEntries[expectedEntries - 2];

            await _handler.SyncCachedDataAsync(randomEntry).ConfigureAwait(false);

            // Assert.
            var firstKeySet = firstCreatedEntry.OriginalKeySet;

            Assert.DoesNotContain(firstKeySet, sendingKeySets);
        }

        [Fact]
        public async Task RenewalCollectsAtMostSyncBatchSizePlusOne()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 1;

            var createdCacheEntries = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var randomEntry = createdCacheEntries[expectedEntries - 2];

            await _handler.SyncCachedDataAsync(randomEntry).ConfigureAwait(false);

            // Assert.
            Assert.Equal(_options.SyncBatchSize + 1, sendingKeySets.Count);
        }

        [Fact]
        public async Task RenewalDoesNotCollectEntriesUnmetRenewalTime()
        {
            // Arrange.
            const int expectedEntries = 4;
            const int assumedCreationInterval = 1;

            var createdCacheEntries = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var firstCreatedEntry = createdCacheEntries[0];

            await _handler.SyncCachedDataAsync(firstCreatedEntry).ConfigureAwait(false);

            // Assert.
            var unmetRenewalEntry = createdCacheEntries[expectedEntries - 1];

            Assert.False(unmetRenewalEntry.NeedToRenew(_clock.UtcNow));
            Assert.DoesNotContain(unmetRenewalEntry.OriginalKeySet, sendingKeySets);
        }

        [Fact]
        public async Task AllEntriesMetRenewalTimeAreCollectedIfSyncBatchSizeIsNotSatisfied()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 2;

            var createdCacheEntries = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };
            _options.SyncBatchSize = 10;

            // Act.
            var lastCreatedEntry = createdCacheEntries[expectedEntries - 1];

            Assert.True(lastCreatedEntry.NeedToRenew(_clock.UtcNow));
            await _handler.SyncCachedDataAsync(lastCreatedEntry).ConfigureAwait(false);

            // Assert.
            Assert.Equal(expectedEntries, sendingKeySets.Count);
        }

        [Fact]
        public async Task EmptyResponseDoesNotUpdateDataInStorage()
        {
            // Arrange.
            var createdEntry = SetupStorageWithOnlyOneEntry();
            var key = createdEntry.OriginalKeySet.BuildKey();
            var emptyValue = new Dictionary<string, TranslationSet>();

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) => Task.FromResult<IDictionary<string, TranslationSet>>(emptyValue);
            _storage.TryGet(key, out CacheEntry oldEntry);

            // Act.
            await _handler.SyncCachedDataAsync(createdEntry).ConfigureAwait(false);

            // Assert.
            _storage.TryGet(key, out CacheEntry currentEntry);

            Assert.Same(oldEntry.Value, currentEntry.Value);
        }

        [Fact]
        public async Task EmptyResponseLetsRenewingLocksBeReleased()
        {
            // Arrange.
            var createdEntry = SetupStorageWithOnlyOneEntry();
            var key = createdEntry.OriginalKeySet.BuildKey();
            var emptyValue = new Dictionary<string, TranslationSet>();

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) => Task.FromResult<IDictionary<string, TranslationSet>>(emptyValue);
            _storage.TryGet(key, out CacheEntry oldEntry);

            // Act.
            Assert.True(createdEntry.NeedToRenew(_clock.UtcNow));
            await _handler.SyncCachedDataAsync(createdEntry).ConfigureAwait(false);

            // Assert.
            _storage.TryGet(key, out CacheEntry currentEntry);

            Assert.True(createdEntry.NeedToRenew(_clock.UtcNow));
        }

        [Fact]
        public async Task NonemptyResponseUpdatesDataInStorage()
        {
            // Arrange.
            var createdEntry = SetupStorageWithOnlyOneEntry();
            var key = createdEntry.OriginalKeySet.BuildKey();
            var newValue = TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = "NewStupidText"
            });
            var syncedData = new Dictionary<string, TranslationSet>
            {
                [key] = newValue
            };

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) => Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);

            // Act.
            await _handler.SyncCachedDataAsync(createdEntry).ConfigureAwait(false);

            // Assert.
            _storage.TryGet(key, out CacheEntry currentEntry);

            Assert.Same(newValue, currentEntry.Value);
        }

        [Fact]
        public async Task LockedEntryIsReleasedIfNotInResponse()
        {
            // Arrange.
            const int expectedEntries = 2;
            const int assumedCreationInterval = 2;

            var createdCacheEntries = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            var lastEntry = createdCacheEntries[expectedEntries - 1];
            var lastEntryKey = lastEntry.OriginalKeySet.BuildKey();
            var newValue = TranslationSet.FromDictionary(new Dictionary<string, string>
            {
                [RefDataTypeHandlerOptions.DefaultLanguage] = "NewStupidText"
            });
            var syncedData = new Dictionary<string, TranslationSet>
            {
                [lastEntryKey] = newValue
            };
            IList<KeySet> sendingKeySets = null;

            _clock.Add(_timeToTriggerRenewal);
            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;

                return Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);
            };

            // Act.
            await _handler.SyncCachedDataAsync(lastEntry).ConfigureAwait(false);

            // Assert.
            var firstEntry = createdCacheEntries[0];

            _storage.TryGet(firstEntry.OriginalKeySet.BuildKey(), out CacheEntry entry);

            Assert.True(entry.NeedToRenew(_clock.UtcNow));
        }

        private CacheEntry[] SetupStorageWithBatchOfData(int expectedLength, int creationInterval)
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

            return createdEntries.ToArray();
        }

        private CacheEntry SetupStorageWithOnlyOneEntry()
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

            return entry;
        }

        public void Dispose()
        {
            _storage.Dispose();
            _storage = null;
        }
    }
}