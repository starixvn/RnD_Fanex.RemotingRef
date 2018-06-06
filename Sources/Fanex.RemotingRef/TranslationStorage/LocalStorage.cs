namespace Fanex.RemotingRef.TranslationStorage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class LocalStorage : IStorage, IDisposable
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _entries;
        private readonly LocalStorageOptions _options;

        private int _entriesCount;
        private DateTimeOffset _lastExpirationScan;
        private bool _disposed;

        public LocalStorage(LocalStorageOptions options, ISystemClock clock)
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

        public int Count => _entriesCount;

        private ICollection<KeyValuePair<string, CacheEntry>> EntryCollection => _entries as ICollection<KeyValuePair<string, CacheEntry>>;

        public CacheEntry CreateEntry(string key, KeySet keySet, TranslationSet value)
        {
            CheckDisposed();

            return new CacheEntry(
                key,
                keySet,
                value,
                _options.SlidingTimeout,
                _options.RenewalTime,
                Clock.UtcNow);
        }

        public IList<CacheEntry> FilterOutOfDateEntries(int batchSize)
        {
            var outOfDateEntries = new List<CacheEntry>();
            DateTimeOffset now = Clock.UtcNow;
            IEnumerator<CacheEntry> enumerableEntries = _entries.Values.OrderByDescending(entry => entry.LastAccess).GetEnumerator();

            try
            {
                CacheEntry entry;

                while (enumerableEntries.MoveNext() && batchSize > outOfDateEntries.Count)
                {
                    entry = enumerableEntries.Current;

                    if (entry.NeedToRenew(now) && !entry.IsExpired(now))
                    {
                        outOfDateEntries.Add(entry);
                    }
                }
            }
            finally
            {
                enumerableEntries.Dispose();
            }

            return outOfDateEntries;
        }

        public bool TryGet(string key, out CacheEntry result)
        {
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

        public void Add(CacheEntry entry)
        {
            CheckDisposed();
            _entries.TryGetValue(entry.Key, out CacheEntry previousEntry);

            if (!entry.IsExpired(Clock.UtcNow))
            {
                TryAddOrUpdateEntry(entry, previousEntry);
            }
            else
            {
                if (previousEntry != null)
                {
                    RemoveEntry(previousEntry);
                }
            }

            if (_entriesCount >= _options.NumberOfEntriesLimit)
            {
                TriggerOvercapacityCompaction();
            }
            else
            {
                StartScanForExpiredEntries();
            }
        }

        public void TryAddOrUpdateEntry(CacheEntry entry, CacheEntry previousEntry)
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
                if (!_entries.TryUpdate(entry.Key, entry, previousEntry) && _entries.TryAdd(entry.Key, entry))
                {
                    Interlocked.Increment(ref _entriesCount);
                }
            }
        }

        private void TriggerOvercapacityCompaction()
            => ThreadPool.QueueUserWorkItem(state => DoMemoryPreassureCollection(state), this);

        private void StartScanForExpiredEntries()
        {
            DateTimeOffset now = Clock.UtcNow;

            if (_options.ExpirationScanFrequency < now - _lastExpirationScan)
            {
                _lastExpirationScan = now;

                Task.Run(() => ScanForExpiredEntries(this));
            }
        }

        private static void ScanForExpiredEntries(LocalStorage storage)
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

#pragma warning disable S3241 // Methods should not return values that are never used

        private static bool DoMemoryPreassureCollection(object state)
        {
            if (state is LocalStorage storage && !storage._disposed)
            {
                storage.Compact(storage._options.CompactionPercentage);

                return true;
            }

            return false;
        }

#pragma warning restore S3241 // Methods should not return values that are never used

        public void Compact(double percentage)
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
            if (EntryCollection.Remove(new KeyValuePair<string, CacheEntry>(entry.Key, entry)))
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
                throw new ObjectDisposedException(typeof(LocalStorage).FullName);
            }
        }
    }
}