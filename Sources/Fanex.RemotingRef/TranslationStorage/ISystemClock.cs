namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    internal interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}