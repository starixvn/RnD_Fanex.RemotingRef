namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    public class MemoryStorageOptions
    {
        public TimeSpan TimeToLive { get; set; }
            = TimeSpan.FromMinutes(10);

        public TimeSpan TimeToRenew { get; set; }
            = TimeSpan.FromSeconds(40);

        public int EntriesLimit { get; set; }
            = 1000;

        public bool CompactOnMemoryPressure { get; set; }

        public TimeSpan ExpirationScanFrequency { get; set; }
            = TimeSpan.FromMinutes(1);

        public double CompactPercentage { get; set; }
            = 0.1;
    }
}