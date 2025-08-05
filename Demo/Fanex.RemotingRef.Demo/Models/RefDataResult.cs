namespace Fanex.RemotingRef.Demo.Models
{
    public class RefDataResult
    {
        public RefDataResult()
        {
            IsDataMatched = true;
        }

        public string FanexRefNew { get; set; }

        public bool IsDataMatched { get; set; }
    }
}