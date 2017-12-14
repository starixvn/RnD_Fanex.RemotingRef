namespace Fanex.RemotingRef.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class MemoryStorage : IStorage, IDisposable
    {
        private const TaskCreationOptions TaskCreationOption = TaskCreationOptions.DenyChildAttach;

        private readonly ConcurrentDictionary<string, CacheEntry> _entries;
        private readonly ISystemClock _clock;
        private readonly MemoryStorageOptions _options;

        private DateTimeOffset _lastExpirationScan;
        private bool _disposed;

        public MemoryStorage(MemoryStorageOptions options, ISystemClock clock)
        {
            _clock = clock ?? new SystemClock();
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _entries = new ConcurrentDictionary<string, CacheEntry>();
            _lastExpirationScan = _clock.UtcNow;

            if (options.CompactOnMemoryPressure)
            {
                GcNotification.Register(DoMemoryPreassureCollection, this);
            }
        }

        private ICollection<KeyValuePair<string, CacheEntry>> EntriesCollection => _entries;

        public CacheEntry CreateEntry(string key, IDictionary<string, string> value)
        {
            CheckDisposed();

            return new CacheEntry(
                key,
                value,
                _options.TimeToLive,
                _options.TimeToRenew,
                _clock.UtcNow);
        }

        public IList<CacheEntry> FilterOutOfDateEntries()
        {
            var outOfDateEntries = new List<CacheEntry>();

            DateTimeOffset now = _clock.UtcNow;

            foreach (var entry in _entries.Values)
            {
                if (entry.NeedToRenew(now))
                {
                    outOfDateEntries.Add(entry);
                }
            }

            return outOfDateEntries;
        }

        public bool TryGet(string key, out IDictionary<string, string> result, out bool needToRenew)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            CheckDisposed();

            result = null;
            needToRenew = false;

            var now = _clock.UtcNow;
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
                    result = entry.Value;
                    needToRenew = entry.NeedToRenew(now);
                    found = true;
                }
            }

            StartScanForExpiredEntries();

            return found;
        }

        public void Set(string key, IDictionary<string, string> value)
        {
            if (_disposed)
            {
                // No-op instead of throwing since this is called during CacheEntry.Dispose
                return;
            }

            DateTimeOffset now = _clock.UtcNow;
            var entry = CreateEntry(key, value);

            _entries.TryGetValue(key, out CacheEntry previousEntry);

            if (!entry.IsExpired(now))
            {
                TryAddOrUpdateEntry(entry, previousEntry);

                if (_entries.Count == _options.EntriesLimit)
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

        internal void TryAddOrUpdateEntry(CacheEntry entry, CacheEntry previousEntry)
        {
            if (previousEntry == null)
            {
                _entries.TryAdd(entry.Key, entry);
            }
            else
            {
                if (!_entries.TryUpdate(entry.Key, entry, previousEntry))
                {
                    _entries.TryAdd(entry.Key, entry);
                }
            }
        }

        private void TriggerOvercapacityCompaction()
            => ThreadPool.QueueUserWorkItem(state => DoMemoryPreassureCollection(state), this);

        private static void ScanForExpiredEntries(MemoryStorage storage)
        {
            DateTimeOffset now = storage._clock.UtcNow;

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
            DateTimeOffset now = _clock.UtcNow;

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
            var removalCountTarget = (int)(_entries.Count * percentage);
            var entriesToRemove = CollectExpiredEntries();

            ExpirePriorityBucket(removalCountTarget, ref entriesToRemove);

            foreach (CacheEntry entry in entriesToRemove)
            {
                RemoveEntry(entry);
            }
        }

        private void ExpirePriorityBucket(int removalCountTarget, ref List<CacheEntry> entriesToRemove)
        {
            if (removalCountTarget <= entriesToRemove.Count)
            {
                return;
            }

            IEnumerator<CacheEntry> rarelyAccessedEntries = _entries.Values.OrderBy(entry => entry.LastAccess).GetEnumerator();

            while (rarelyAccessedEntries.MoveNext() && removalCountTarget > entriesToRemove.Count)
            {
                CacheEntry entry = rarelyAccessedEntries.Current;
                entry.Expire();
                entriesToRemove.Add(entry);
            }
        }

        private List<CacheEntry> CollectExpiredEntries()
        {
            var expiredEntries = new List<CacheEntry>();

            DateTimeOffset now = _clock.UtcNow;

            foreach (var entry in _entries.Values)
            {
                if (entry.IsExpired(now))
                {
                    expiredEntries.Add(entry);
                }
            }

            return expiredEntries;
        }

        private void RemoveEntry(CacheEntry entry)
            => EntriesCollection.Remove(new KeyValuePair<string, CacheEntry>(entry.Key, entry));

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