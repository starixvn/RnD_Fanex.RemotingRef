namespace Fanex.RemotingRef.Tests.TranslationStorage
{
    using System;
    using RemotingRef.TranslationStorage;

    internal class TestClock : ISystemClock
    {
        public TestClock()
            => UtcNow = new DateTimeOffset(2017, 12, 12, 12, 34, 56, 789, new TimeSpan(0, 0, 0));

        public DateTimeOffset UtcNow { get; private set; }

        public void Add(TimeSpan addedTime)
            => UtcNow = UtcNow + addedTime;
    }
}