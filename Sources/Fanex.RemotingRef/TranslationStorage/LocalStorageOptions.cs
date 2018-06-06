namespace Fanex.RemotingRef.TranslationStorage
{
    using System;

    public class LocalStorageOptions
    {
        private int _numberOfEntriesLimit = 1000;
        private double _compactionPercentage = 0.1;
        private TimeSpan _slidingTimeout = TimeSpan.FromMinutes(10);
        private TimeSpan _renewalTime = TimeSpan.FromSeconds(40);

        public TimeSpan SlidingTimeout
        {
            get => _slidingTimeout;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "The sliding expiration value must be positive.");
                }

                _slidingTimeout = value;
            }
        }

        public TimeSpan RenewalTime
        {
            get => _renewalTime;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "The recycling interval value must be positive.");
                }

                _renewalTime = value;
            }
        }

        public int NumberOfEntriesLimit
        {
            get => _numberOfEntriesLimit;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "Number of entries of each data type must be positive.");
                }

                _numberOfEntriesLimit = value;
            }
        }

        public bool CompactOnMemoryPressure { get; set; }

        public TimeSpan ExpirationScanFrequency { get; set; }
            = TimeSpan.FromMinutes(1);

        public double CompactionPercentage
        {
            get => _compactionPercentage;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "Compaction Percentage must be between 0 and 1 inclusive.");
                }

                _compactionPercentage = value;
            }
        }
    }
}