namespace Fanex.RemotingRef.ExposedModels
{
    public class ExchangeData
    {
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public double ActualRate { get; set; }

        public double ComMaxPayout { get; set; }

        public double Minbet { get; set; }

        public double MinbetLive { get; set; }

        public double AUDRate { get; set; }

        public double ZARRate { get; set; }

        public double GBPRate { get; set; }

        public double EURORate { get; set; }
    }
}