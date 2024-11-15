namespace Fanex.RemotingRef.TranslationStorage;

using System;

internal class CacheEntry(
    string key,
    KeySet keySet,
    TranslationSet value,
    TimeSpan slidingTimeout,
    TimeSpan renewalTime,
    DateTimeOffset createdTime)
{
    private bool _isExpired;
    private bool _isRenewing;

    public string Key { get; } = key ?? throw new ArgumentNullException(nameof(key));

    public KeySet OriginalKeySet { get; } = keySet ?? throw new ArgumentNullException(nameof(keySet));

    public TranslationSet Value { get; } = value;

    public TimeSpan SlidingTimeout { get; } = slidingTimeout;

    public TimeSpan RenewalTime { get; } = renewalTime;

    public DateTimeOffset CreatedTime { get; private set; } = createdTime;

    public DateTimeOffset LastAccess { get; private set; } = createdTime;

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
        if (now - CreatedTime > RenewalTime)
        {
            _isRenewing = true;

            return true;
        }

        return false;
    }

    public bool ReleaseRenewingLock()
        => _isRenewing = false;
}