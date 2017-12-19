namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    public class CacheEntry
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

        public DateTimeOffset CreatedTime { get; private set; }

        public TranslationSet Value { get; }

        public DateTimeOffset LastAccess { get; private set; }

        public TimeSpan SlidingTimeout { get; }

        public TimeSpan RecyclingInterval { get; }

        public void Touch(DateTimeOffset now)
            => LastAccess = now;

        public void Expire()
            => _isExpired = true;

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