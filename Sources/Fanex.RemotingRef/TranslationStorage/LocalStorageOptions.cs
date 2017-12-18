namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    public class LocalStorageOptions
    {
        public TimeSpan SlidingTimeout { get; set; }
            = TimeSpan.FromMinutes(10);

        public TimeSpan RecyclingInterval { get; set; }
            = TimeSpan.FromSeconds(40);

        public int NumberOfEntriesLimit { get; set; }
            = 1000;

        public bool CompactOnMemoryPressure { get; set; }

        public TimeSpan ExpirationScanFrequency { get; set; }
            = TimeSpan.FromMinutes(1);

        public double CompactPercentage { get; set; }
            = 0.1;
    }
}