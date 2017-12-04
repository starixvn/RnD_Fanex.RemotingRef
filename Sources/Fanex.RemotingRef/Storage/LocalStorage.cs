namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LocalStorage : IDisposable
    {
        private const TaskCreationOptions TaskCreationOption = TaskCreationOptions.DenyChildAttach;
        private const double CompactPercentage = 0.1;

        private readonly ConcurrentDictionary<string, CacheEntry> _entries;
        private readonly ISystemClock _clock;
        private readonly TimeSpan _expirationScanFrequency;

        private DateTimeOffset _lastExpirationScan;
        private bool _disposed;

        internal LocalStorage(LocalStorageOptions options, ISystemClock clock)
        {
            _entries = new ConcurrentDictionary<string, CacheEntry>();
            _clock = clock;
            _expirationScanFrequency = options.ExpirationScanFrequency;
            _lastExpirationScan = _clock.UtcNow;

            if (options.CompactOnMemoryPressure)
            {
                GcNotification.Register(DoMemoryPreassureCollection, this);
            }
        }

        public TimeSpan TimeToLive { get; }

        public TimeSpan TimeToUpdate { get; }

        internal int Count => _entries.Count;

        public void Dispose()
            => _disposed = true;

        public bool Contain(string key)
        {
            CheckDisposed();

            return _entries.TryGetValue(key, out _);
        }

        public bool TryGet(string key, out object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var found = false;
            DateTimeOffset now = _clock.UtcNow;

            CheckDisposed();
            value = null;

            if (_entries.TryGetValue(key, out CacheEntry entry))
            {
                if (entry.IsExpired(now))
                {
                    RemoveEntry(entry);
                }
                else
                {
                    entry.Touch(now);
                    value = entry.Value;
                    found = true;
                }
            }

            StartScanForExpiredEntries();

            return found;
        }

        public void Set(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_disposed)
            {
                // No-op instead of throwing since this is called during CacheEntry.Dispose
                return;
            }

            DateTimeOffset now = _clock.UtcNow;
            var entry = new CacheEntry(key, value, TimeToLive, TimeToUpdate, now);

            if (_entries.TryGetValue(key, out CacheEntry previousEntry))
            {
                previousEntry.Expire();
            }

            if (!entry.IsExpired(now))
            {
                TryAddOrUpdateEntry(entry, previousEntry);
            }
            else if (previousEntry != null)
            {
                RemoveEntry(previousEntry);
            }

            StartScanForExpiredEntries();
        }

        internal void TryAddOrUpdateEntry(CacheEntry entry, CacheEntry previousEntry)
        {
            bool added;

            if (previousEntry == null)
            {
                added = _entries.TryAdd(entry.Key, entry);
            }
            else
            {
                added = _entries.TryUpdate(entry.Key, entry, previousEntry);

                if (!added)
                {
                    added = _entries.TryAdd(entry.Key, entry);
                }
            }

            if (!added)
            {
                entry.Expire();
            }
        }

        private void StartScanForExpiredEntries()
        {
            DateTimeOffset now = _clock.UtcNow;

            if (_expirationScanFrequency < now - _lastExpirationScan)
            {
                _lastExpirationScan = now;

                Task.Factory.StartNew(
                    state => ScanForExpiredEntries((LocalStorage)state),
                    this,
                    CancellationToken.None,
                    TaskCreationOption,
                    TaskScheduler.Default);
            }
        }

        private static void ScanForExpiredEntries(LocalStorage cache)
        {
            DateTimeOffset now = cache._clock.UtcNow;

            foreach (CacheEntry entry in cache._entries.Values)
            {
                if (entry.IsExpired(now))
                {
                    cache.RemoveEntry(entry);
                }
            }
        }

        private bool DoMemoryPreassureCollection(object state)
        {
            if (_disposed)
            {
                return false;
            }

            if (state is LocalStorage storage)
            {
                storage.Compact(CompactPercentage);

                return true;
            }

            return false;
        }

        internal void Compact(double percentage)
        {
            var expiredEntries = new List<CacheEntry>();
            var lowPriorityEntries = new List<CacheEntry>();
            var normalPriorityEntries = new List<CacheEntry>();
            var highPriorityEntries = new List<CacheEntry>();
            var removalCountTarget = (int)(_entries.Count * percentage);

            CategorizeEntries(
                    ref expiredEntries,
                    ref lowPriorityEntries,
                    ref normalPriorityEntries,
                    ref highPriorityEntries);

            ExpirePriorityBucket(removalCountTarget, expiredEntries, lowPriorityEntries);
            ExpirePriorityBucket(removalCountTarget, expiredEntries, normalPriorityEntries);
            ExpirePriorityBucket(removalCountTarget, expiredEntries, highPriorityEntries);

            foreach (CacheEntry entry in expiredEntries)
            {
                RemoveEntry(entry);
            }
        }

        private void CategorizeEntries(
            ref List<CacheEntry> expiredEntries,
            ref List<CacheEntry> lowPriorityEntries,
            ref List<CacheEntry> normalPriorityEntries,
            ref List<CacheEntry> highPriorityEntries)
        {
            DateTimeOffset now = _clock.UtcNow;

            foreach (var entry in _entries.Values)
            {
                if (entry.IsExpired(now))
                {
                    expiredEntries.Add(entry);
                }
                else
                {
                    switch (entry.Priority)
                    {
                        case CachePriority.Low:
                            lowPriorityEntries.Add(entry);
                            break;

                        case CachePriority.Normal:
                            normalPriorityEntries.Add(entry);
                            break;

                        case CachePriority.High:
                            highPriorityEntries.Add(entry);
                            break;

                        case CachePriority.NeverRemove:
                            break;
                    }
                }
            }
        }

        private static void ExpirePriorityBucket(
            int removalCountTarget,
            List<CacheEntry> expiredEntries,
            List<CacheEntry> priorityEntries)
        {
            if (removalCountTarget <= expiredEntries.Count)
            {
                return;
            }

            if (expiredEntries.Count + priorityEntries.Count <= removalCountTarget)
            {
                foreach (var entry in priorityEntries)
                {
                    entry.Expire();
                }

                expiredEntries.AddRange(priorityEntries);

                return;
            }

            IEnumerable<CacheEntry> orderedPriorityEntries = priorityEntries.OrderBy(entry => entry.LastTouch);

            foreach (CacheEntry entry in orderedPriorityEntries)
            {
                entry.Expire();
                expiredEntries.Add(entry);

                if (removalCountTarget <= expiredEntries.Count)
                {
                    break;
                }
            }
        }

        private void RemoveEntry(CacheEntry entry)
            => _entries.TryRemove(entry.Key, out _);

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(LocalStorage).FullName);
            }
        }
    }
}