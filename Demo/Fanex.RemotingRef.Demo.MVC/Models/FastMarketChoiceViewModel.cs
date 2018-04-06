namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Models;

    public class FastMarketChoiceViewModel : RefDataViewModel
    {
        public string BetTeam { get; set; }

        public int Handicap1 { get; set; }

        public int Handicap2 { get; set; }
    }
}