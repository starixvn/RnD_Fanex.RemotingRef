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

    public class SyncingTranslationSetsTests
    {
        private readonly IRefDataTypeHandler _handler;
        private readonly RefDataTypeHandlerOptions _options;
        private readonly LocalStorage _storage;
        private readonly TestClock _clock;

        public SyncingTranslationSetsTests()
        {
            _options = new RefDataTypeHandlerOptions();
            _clock = new TestClock();
            _storage = new LocalStorage(_options, _clock);
            _handler = new RefDataTypeHandler(_options, _storage);

            _options.RecyclingInterval = TimeSpan.FromSeconds(1);
            _options.SyncBatchSize = 2;
        }

        [Fact]
        public void NegativeRecyclingIntervalThrowsArgumentOutOfRangeException()
        {
            // Arrange.
            var negativeRecyclingInterval = TimeSpan.FromSeconds(-1);

            // Act.
            Exception exception = Record.Exception(() => _options.RecyclingInterval = negativeRecyclingInterval);

            // Assert.
            Assert.IsType<ArgumentOutOfRangeException>(exception);
        }

        [Fact]
        public void RetrievedEntryMetRenewalTimeTriggersSyncProcess()
        {
            // Arrange.
            var existingKeySet = SetupStorageWithOnlyOneEntry();
            var callbackInvoked = new ManualResetEvent(false);

            _clock.Add(_options.RecyclingInterval + TimeSpan.FromSeconds(1));
            _options.SyncHandler = (parameterSets) =>
            {
                callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            _handler.Get(existingKeySet, null);

            // Assert.
            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)));
        }

        [Fact]
        public void RenewalPrioritizesLatestAccessEntries()
        {
            // Arrange.
            const int expectedEntries = 4;
            const int assumedCreationInterval = 2;
            var callbackInvoked = new ManualResetEvent(false);
            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };
            _clock.Add(TimeSpan.FromSeconds(30));

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            var firsKeySet = createdKeySets[0];

            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)));
            Assert.DoesNotContain(firsKeySet, sendingKeySets);
        }

        [Fact]
        public void RenewalCollectsAtMostSyncBatchSizePlusOne()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 2;
            var callbackInvoked = new ManualResetEvent(false);
            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)));
            Assert.Equal(_options.SyncBatchSize + 1, sendingKeySets.Count);
        }

        [Fact]
        public void AllEntriesMetRenewalTimeAreCollectedIfSyncBatchSizeIsNotSatisfied()
        {
            // Arrange.
            const int expectedEntries = 5;
            const int assumedCreationInterval = 2;
            var callbackInvoked = new ManualResetEvent(false);
            var createdKeySets = SetupStorageWithBatchOfData(expectedEntries, assumedCreationInterval);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                callbackInvoked.Set();

                return Task.FromResult<IDictionary<string, TranslationSet>>(null);
            };
            _options.SyncBatchSize = 10;

            // Act.
            var lastKeySet = createdKeySets[expectedEntries - 1];

            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(callbackInvoked.WaitOne(TimeSpan.FromSeconds(30)));
            Assert.Equal(expectedEntries, sendingKeySets.Count);
        }

        [Fact]
        public void EmptyResponseLetsRenewingLocksBeReleased()
        {
            // Arrange.
            var createdKeySet = SetupStorageWithOnlyOneEntry();
            var key = createdKeySet.BuildKey();

            _options.SyncHandler = (parameterSets) => Task.FromResult<IDictionary<string, TranslationSet>>(new Dictionary<string, TranslationSet>());
            _storage.TryGet(key, out CacheEntry oldEntry);

            // Act.
            _handler.Get(createdKeySet, null);

            // Assert.
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
            var sem = new SemaphoreSlim(0, 1);

            _options.SyncHandler = (parameterSets) =>
            {
                sem.Release();

                return Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);
            };
            _clock.Add(TimeSpan.FromSeconds(30));

            // Act.
            _handler.Get(createdKeySet, null);

            // Assert.
            Assert.True(await sem.WaitAsync(TimeSpan.FromSeconds(10)));

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
            var sem = new SemaphoreSlim(0, 1);
            IList<KeySet> sendingKeySets = null;

            _options.SyncHandler = (parameterSets) =>
            {
                sendingKeySets = parameterSets;
                sem.Release();

                return Task.FromResult<IDictionary<string, TranslationSet>>(syncedData);
            };
            _clock.Add(TimeSpan.FromSeconds(30));

            // Act.
            _handler.Get(lastKeySet, null);

            // Assert.
            Assert.True(await sem.WaitAsync(TimeSpan.FromSeconds(10)));

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
    }
}