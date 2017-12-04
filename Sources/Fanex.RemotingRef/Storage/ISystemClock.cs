namespace Fanex.RemotingRef.Storage
{
    using System;

    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}