namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    internal class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}