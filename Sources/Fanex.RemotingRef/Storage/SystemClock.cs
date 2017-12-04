namespace Fanex.RemotingRef.Storage
{
    using System;

    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}