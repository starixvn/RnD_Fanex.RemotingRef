namespace Fanex.RemotingRef.Storage
{
    using System;
    using System.Collections.Generic;

    internal class CacheEntry
    {
        private bool _isExpired;
        private bool _isRenewing;

        public CacheEntry(
            string key,
            IDictionary<string, string> value,
            TimeSpan timeToLive,
            TimeSpan timeToRenew,
            DateTimeOffset createdTime)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
            TimeToLive = timeToLive;
            TimeToRenew = timeToRenew;
            CreatedTime = createdTime;
            LastAccess = createdTime;
        }

        public string Key { get; }

        public IDictionary<string, string> Value { get; }

        public DateTimeOffset CreatedTime { get; private set; }

        public DateTimeOffset LastAccess { get; private set; }

        public TimeSpan TimeToLive { get; }

        public TimeSpan TimeToRenew { get; }

        public void Touch(DateTimeOffset now)
            => LastAccess = now;

        public void Expire()
            => _isExpired = true;

        public bool IsExpired(DateTimeOffset now)
            => _isExpired || MeetExpirationTime(now);

        private bool MeetExpirationTime(DateTimeOffset now)
        {
            if (now - LastAccess > TimeToLive)
            {
                Expire();

                return true;
            }

            return false;
        }

        public bool NeedToRenew(DateTimeOffset now)
            => !_isRenewing && IsFirstRenewal(now);

        private bool IsFirstRenewal(DateTimeOffset now)
        {
            if (now - CreatedTime > TimeToRenew)
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