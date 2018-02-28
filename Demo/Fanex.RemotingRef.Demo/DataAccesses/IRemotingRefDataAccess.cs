namespace Fanex.RemotingRef.Demo.DataAccesses
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public interface IRemotingRefDataAccess
    {
        RefDataResult GetBetTypeName(string betTypeId, long refNo = 0, string resourceId = Common.DefaultResourceId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetSportName(string sportTypeId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetLeagueName(string leagueId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetTeamName(string teamId, string languageCode = Common.DefaultLanguage);

        RefDataResult GetResourceData(string refId, string resourceId, string languageCode = Common.DefaultLanguage);
    }
}