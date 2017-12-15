namespace Fanex.RemotingRef.TranslationStorage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class MemoryStorage : IStorage, IDisposable
    {
        private const TaskCreationOptions TaskCreationOption = TaskCreationOptions.DenyChildAttach;

        private readonly ConcurrentDictionary<string, CacheEntry> _entries;
        private readonly MemoryStorageOptions _options;

        private int _entriesCount;
        private DateTimeOffset _lastExpirationScan;
        private bool _disposed;

        public MemoryStorage(MemoryStorageOptions options)
            : this(options, new SystemClock())
        {
        }

        public MemoryStorage(MemoryStorageOptions options, ISystemClock clock)
        {
            Clock = clock ?? new SystemClock();
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _entries = new ConcurrentDictionary<string, CacheEntry>();
            _lastExpirationScan = Clock.UtcNow;

            if (options.CompactOnMemoryPressure)
            {
                GcNotification.Register(DoMemoryPreassureCollection, this);
            }
        }

        public ISystemClock Clock { get; }

        public CacheEntry CreateEntry(string key, TranslationSet value, ExtraDataSet extraData)
        {
            CheckDisposed();

            return new CacheEntry(
                key,
                value,
                extraData,
                _options.TimeToLive,
                _options.TimeToRenew,
                Clock.UtcNow);
        }

        public IList<CacheEntry> FilterOutOfDateEntries(int batchSize)
        {
            var outOfDateEntries = new List<CacheEntry>();
            DateTimeOffset now = Clock.UtcNow;
            IEnumerator<CacheEntry> enumerableValues = _entries.Values.GetEnumerator();

            try
            {
                CacheEntry entry;

                while (enumerableValues.MoveNext() && batchSize > outOfDateEntries.Count)
                {
                    entry = enumerableValues.Current;

                    if (entry.NeedToRenew(now))
                    {
                        outOfDateEntries.Add(entry);
                    }
                }
            }
            finally
            {
                enumerableValues.Dispose();
            }

            return outOfDateEntries;
        }

        public bool TryGet(string key, out CacheEntry result)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            CheckDisposed();

            result = null;

            var now = Clock.UtcNow;
            var found = false;

            if (_entries.TryGetValue(key, out CacheEntry entry))
            {
                if (entry.IsExpired(now))
                {
                    RemoveEntry(entry);
                }
                else
                {
                    entry.Touch(now);
                    result = entry;
                    found = true;
                }
            }

            StartScanForExpiredEntries();

            return found;
        }

        public void Add(string key, CacheEntry entry)
        {
            if (_disposed)
            {
                // No-op instead of throwing since this is called during CacheEntry.Dispose
                return;
            }

            _entries.TryGetValue(key, out CacheEntry previousEntry);

            if (!entry.IsExpired(Clock.UtcNow))
            {
                TryAddOrUpdateEntry(entry, previousEntry);

                if (_entriesCount > _options.EntriesLimit)
                {
                    TriggerOvercapacityCompaction();
                }
            }
            else
            {
                if (previousEntry != null)
                {
                    RemoveEntry(previousEntry);
                }
            }

            StartScanForExpiredEntries();
        }

        public void TryUpdate(CacheEntry entry, CacheEntry previousEntry)
            => _entries.TryUpdate(entry.Key, entry, previousEntry);

        internal void TryAddOrUpdateEntry(CacheEntry entry, CacheEntry previousEntry)
        {
            if (previousEntry == null)
            {
                if (_entries.TryAdd(entry.Key, entry))
                {
                    Interlocked.Increment(ref _entriesCount);
                }
            }
            else
            {
                if (!_entries.TryUpdate(entry.Key, entry, previousEntry)
                    && _entries.TryAdd(entry.Key, entry))
                {
                    Interlocked.Increment(ref _entriesCount);
                }
            }
        }

        private void TriggerOvercapacityCompaction()
            => ThreadPool.QueueUserWorkItem(state => DoMemoryPreassureCollection(state), this);

        private static void ScanForExpiredEntries(MemoryStorage storage)
        {
            DateTimeOffset now = storage.Clock.UtcNow;

            foreach (CacheEntry entry in storage._entries.Values)
            {
                if (entry.IsExpired(now))
                {
                    storage.RemoveEntry(entry);
                }
            }
        }

        private void StartScanForExpiredEntries()
        {
            DateTimeOffset now = Clock.UtcNow;

            if (_options.ExpirationScanFrequency < now - _lastExpirationScan)
            {
                _lastExpirationScan = now;

                Task.Factory.StartNew(
                    state => ScanForExpiredEntries(state as MemoryStorage),
                    this,
                    CancellationToken.None,
                    TaskCreationOption,
                    TaskScheduler.Default);
            }
        }

#pragma warning disable S3241 // Methods should not return values that are never used

        private bool DoMemoryPreassureCollection(object state)
        {
            if (_disposed)
            {
                return false;
            }

            if (state is MemoryStorage storage)
            {
                storage.Compact(_options.CompactPercentage);

                return true;
            }

            return false;
        }

#pragma warning restore S3241 // Methods should not return values that are never used

        internal void Compact(double percentage)
        {
            var removalCountTarget = (int)(_entriesCount * percentage);
            var entriesToRemove = CollectRarelyAccessedBucket(removalCountTarget);

            foreach (CacheEntry entry in entriesToRemove)
            {
                RemoveEntry(entry);
            }
        }

        private List<CacheEntry> CollectRarelyAccessedBucket(int removalCountTarget)
        {
            var entriesToRemove = new List<CacheEntry>();
            IEnumerator<CacheEntry> rarelyAccessedEntries = _entries.Values.OrderBy(entry => entry.LastAccess).GetEnumerator();

            try
            {
                CacheEntry entry;

                while (rarelyAccessedEntries.MoveNext() && removalCountTarget > entriesToRemove.Count)
                {
                    entry = rarelyAccessedEntries.Current;
                    entry.Expire();
                    entriesToRemove.Add(entry);
                }
            }
            finally
            {
                rarelyAccessedEntries.Dispose();
            }

            return entriesToRemove;
        }

        private void RemoveEntry(CacheEntry entry)
        {
            if (_entries.TryRemove(entry.Key, out _))
            {
                Interlocked.Decrement(ref _entriesCount);
            }
        }

        public void Dispose()
            => _disposed = true;

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(MemoryStorage).FullName);
            }
        }
    }
}