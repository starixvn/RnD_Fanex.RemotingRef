namespace Fanex.RemotingRef.NetCore.Utilities.RefData
{
    public interface IRefDataHelper
    {
        string GetBetTypeName(
            string betTypeId,
            string resourceId,
            long? refNo,
            string language);

        string GetSportName(string sportTypeId, string language);

        string GetTeamName(string teamId, string language);

        string GetTeamName3rd(string team3rdId, string language);

        string GetLeagueName(string leagueId, string language);

        string GetLeagueName3rd(string league3rdId, string language);

        string GetCountryName(string countryCode, string language);

        string GetOddsTypeName(string oddsTypeId, string language);

        string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string language);

        string GetParentIdByBetTypeId(string betTypeId);

        string[] GetCombinationTypeCombo(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            string language);

        string GetHorseTeamName(string codeId, string language);

        string GetCasinoBetItem(string itemId, string language);

        string GetRouletteBetItem(string itemId, string language);


        string GetKenoBetChoice(string kenoBetChoiceId, string language, out string kenoBetTypeId);


        string GetKenoBetType(string kenoBetTypeId, string language);

        string GetFastMarketsChoice(
            string betTypeId,
            string betTeam,
            int handicap1,
            int handicap2,
            string language);

        string GetFastMarketsResult(string resultId, string language);

        string GetFastMarkets225Result(string resultId, string language);


        string GetLotteryBetChoice(string lotteryBetChoiceId, string language, out string lotteryBetTypeId);


        string GetLotteryBetType(string lotteryBetTypeId, string language);

        string GetBetRadarMatchCodeFormat(string matchCode, string language);

        string GetDisplayedTypeId(string betTypeId);

        string GetBetBuilderSelectionName(int selectionId, string language);


        string GetTableGamesBetChoice(string tableGamesBetChoiceId, string language, out string tableGamesBetTypeId);


        string GetTableGamesBetType(string tableGamesBetTypeId, string language);
    }
}