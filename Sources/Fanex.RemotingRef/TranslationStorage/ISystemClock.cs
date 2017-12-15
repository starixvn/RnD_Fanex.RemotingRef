namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}