namespace Fanex.RemotingRef.Tests.Mocks
{
    using System;
    using Fanex.RemotingRef.TranslationStorage;

    internal class TestClock : ISystemClock
    {
        public TestClock()
            => UtcNow = new DateTimeOffset(2017, 12, 12, 12, 34, 56, 789, new TimeSpan(0, 0, 0));

        public DateTimeOffset UtcNow { get; private set; }

        public void Add(TimeSpan addedTime)
            => UtcNow = UtcNow + addedTime;

        public void AddSeconds(double seconds)
            => Add(TimeSpan.FromSeconds(seconds));

        public void AddMinutes(double minutes)
            => Add(TimeSpan.FromMinutes(minutes));
    }
}