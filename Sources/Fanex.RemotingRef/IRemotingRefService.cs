namespace Fanex.RemotingRef
{
    public interface IRemotingRefService
    {
        string GetBetTypeName(string betTypeId, string languageCode);

        string GetBetTypeName(string betTypeId, string languageCode, long refNo);

        string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue);

        string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId);

        string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo);

        string GetSportName(string sportTypeId, string languageCode);

        string GetSportName(string sportTypeId, string languageCode, string defaultValue);

        string GetTeamName(string teamId, string languageCode);

        string GetTeamName(string teamId, string languageCode, string defaultValue);

        string GetTeamName3rd(string team3rdId, string languageCode);

        string GetTeamName3rd(string team3rdId, string languageCode, string defaultValue);

        string GetLeagueName(string leagueId, string languageCode);

        string GetLeagueName(string leagueId, string languageCode, string defaultValue);

        string GetLeagueName3rd(string league3rdId, string languageCode);

        string GetLeagueName3rd(string league3rdId, string languageCode, string defaultValue);

        string GetCountryName(string countryCode, string languageCode);

        string GetCountryName(string countryCode, string languageCode, string defaultValue);

        string GetOddsTypeName(string oddsTypeId, string languageCode);

        string GetOddsTypeName(string oddsTypeId, string languageCode, string defaultValue);

        string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue);

        string GetParentIdByBetTypeId(string betTypeId, string defaultValue);

        string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string languageCode);

        string GetHorseTeamName(string codeId, string languageCode);

        string GetHorseTeamName(string codeId, string languageCode, string defaultValue);

        string GetCasinoBetItem(string itemId, string languageCode);

        string GetCasinoBetItem(string itemId, string languageCode, string defaultValue);

        string GetRouletteBetItem(string itemId, string languageCode);

        string GetRouletteBetItem(string itemId, string languageCode, string defaultValue);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId);

        string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        string GetKenoBetType(string kenoBetTypeId, string languageCode);

        string GetKenoBetType(string kenoBetTypeId, string languageCode, string defaultValue);

        string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode);

        string GetFastMarkets225Result(string resultId, string languageCode);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, out string lotteryBetTypeId);

        string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, string defaultValue, out string lotteryBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        string GetLotteryBetType(string lotteryBetTypeId, string languageCode);

        string GetLotteryBetType(string lotteryBetTypeId, string languageCode, string defaultValue);

        string GetBetRadarMatchCodeFormat(string matchCode, string languageCode);

        string GetDisplayedTypeId(string betTypeId);
    }
}