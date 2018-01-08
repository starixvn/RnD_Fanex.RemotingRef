namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRefDataHelper
    {
        string GetBetTypeName(string betTypeId, string language, string defaultValue);

        string GetBetTypeName(string betTypeId, string language, long refNo, string defaultValue);

        string GetBetTypeName(string betTypeId, string language, string defaultValue, string resourceId);

        string GetBetTypeName(string betTypeId, string language, string defaultValue, string resourceId, long refNo);

        string GetSportName(string sportType, string language, string defaultValue);

        string GetTeamName(string teamId, string language, string defaultValue);

        string GetLeagueName(string leagueId, string language, string defaultValue);

        string GetCountryName(string contryCode, string language, string defaultValue);
    }
}