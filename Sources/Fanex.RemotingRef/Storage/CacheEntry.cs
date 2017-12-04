namespace Fanex.RemotingRef
{
    using System;

    public class CacheEntry
    {
        private bool _isExpired;
        private bool _isUpdating;

        private bool _isTimeToUpdate;

        public CacheEntry(
            string key,
            object value,
            TimeSpan timeToLive,
            TimeSpan timeToRenew,
            DateTimeOffset createdTime)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            TimeToLive = timeToLive;
            TimeToRenew = timeToRenew;
            CreatedTime = createdTime;
            LastTouch = createdTime;
        }

        public string Key { get; }

        public object Value { get; }

        public TimeSpan TimeToLive { get; }

        public TimeSpan TimeToRenew { get; }

        public DateTimeOffset CreatedTime { get; private set; }

        internal DateTimeOffset LastTouch { get; private set; }

        internal CachePriority Priority { get; }

        public void Touch(DateTimeOffset now)
            => LastTouch = now;

        public void Expire()
            => _isExpired = true;

        public bool IsExpired(DateTimeOffset now)
            => _isExpired || CheckForExpiration(now);

        private bool CheckForExpiration(DateTimeOffset now)
        {
            if ((now - LastTouch) >= TimeToLive)
            {
                Expire();

                return true;
            }

            return false;
        }

        public bool IsTimeToUpdate(DateTimeOffset now)
            => _isTimeToUpdate || CheckForUpdateTime(now);

        private bool CheckForUpdateTime(DateTimeOffset now)
        {
            TimeSpan livingTime = now - CreatedTime;

            if (_isUpdating)
            {
                if ((livingTime - TimeSpan.FromMinutes(1)) > TimeToRenew)
                {
                    _isUpdating = false;
                }
                else
                {
                    return false;
                }
            }

            if (livingTime > TimeToRenew)
            {
                _isTimeToUpdate = true;

                return true;
            }

            return false;
        }
    }
}