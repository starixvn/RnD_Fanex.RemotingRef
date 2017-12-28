namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    internal class CacheEntry
    {
        private bool _isExpired;
        private bool _isRenewing;

        public CacheEntry(
            string key,
            KeySet keySet,
            TranslationSet value,
            TimeSpan slidingTimeout,
            TimeSpan recyclingInterval,
            DateTimeOffset createdTime)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            OriginalKeySet = keySet ?? throw new ArgumentNullException(nameof(keySet));
            Value = value;
            SlidingTimeout = slidingTimeout;
            RecyclingInterval = recyclingInterval;
            CreatedTime = createdTime;
            LastAccess = createdTime;
        }

        public string Key { get; }

        public KeySet OriginalKeySet { get; }

        public TranslationSet Value { get; }

        public TimeSpan SlidingTimeout { get; }

        public TimeSpan RecyclingInterval { get; }

        public DateTimeOffset CreatedTime { get; private set; }

        public DateTimeOffset LastAccess { get; private set; }

#if DEBUG

        /// <summary>
        /// Gets or sets action which is invoked once the entry is expired. Being used for unit testing purpose.
        /// </summary>
        public Action PostEvictionCallback { get; set; }

#endif

        public void Touch(DateTimeOffset now)
            => LastAccess = now;

#if DEBUG

        public void Expire()
        {
            _isExpired = true;

            PostEvictionCallback?.Invoke();
        }

#else

        public void Expire()
            => _isExpired = true;

#endif

        public bool IsExpired(DateTimeOffset now)
            => _isExpired || MeetExpirationTime(now);

        private bool MeetExpirationTime(DateTimeOffset now)
        {
            if (now - LastAccess > SlidingTimeout)
            {
                Expire();

                return true;
            }

            return false;
        }

        public bool NeedToRenew(DateTimeOffset now)
            => !_isRenewing && MeetRenewalTime(now);

        private bool MeetRenewalTime(DateTimeOffset now)
        {
            if (now - CreatedTime > RecyclingInterval)
            {
                _isRenewing = true;

                return true;
            }

            return false;
        }

        public bool ReleaseRenewingLock()
            => _isRenewing = false;
    }
}