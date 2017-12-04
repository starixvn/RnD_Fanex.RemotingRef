namespace Fanex.RemotingRef
{
    using System;

    internal class LocalStorageOptions
    {
        public bool CompactOnMemoryPressure { get; set; }
            = true;

        public TimeSpan ExpirationScanFrequency { get; set; }
            = TimeSpan.FromMinutes(1);

        public TimeSpan AliveScanPeriod { get; set; }
            = TimeSpan.FromHours(1);
    }
}