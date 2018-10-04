namespace Fanex.RemotingRef.Demo.DataAccesses
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public interface IRemotingRefDataAccess
    {
        RefDataResult GetBetTypeName(string betTypeId, long refNo = 0, string resourceId = Common.DefaultResourceId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetSportName(string sportTypeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetLeagueName(string leagueId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetLeagueName3rd(string leagueId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetTeamName(string teamId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetTeamName3rd(string teamId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetResourceData(string refId, string resourceId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetCountryName(string countryId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetOddsTypeName(string oddsTypeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetParentIdByBetTypeId(string betTypeId);

        RefDataResult GetHorseTeamName(string codeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetCasinoBetItem(string itemId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetRouletteBetItem(string itemId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetKenoBetType(string kenoBetTypeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode = Common.DefaultLanguage);

        RefDataResult GetFastMarketsResult(string resultId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetFastMarkets225Result(string resultId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetLotteryBetType(string betTypeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetBetRadarMatchCodeFormat(string matchCode, string languageCode = Common.DefaultLanguage);

        RefDataResult GetDisplayedTypeId(string betTypeId);
    }
}