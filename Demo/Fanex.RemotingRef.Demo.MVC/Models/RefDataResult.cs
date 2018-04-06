namespace Fanex.RemotingRef.Demo.Models
{
    public class RefDataResult
    {
        public string TaiwanRefOld { get; set; }

        public string TaiwanRefNew { get; set; }

        public string FanexRefNew { get; set; }

        public bool IsDataMatched()
            => FanexRefNew == TaiwanRefNew &&
               FanexRefNew == TaiwanRefOld &&
               TaiwanRefNew == TaiwanRefOld;
    }
}