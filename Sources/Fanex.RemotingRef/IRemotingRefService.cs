namespace Fanex.RemotingRef
{
    public interface IRemotingRefService
    {
        string GetParentIdByBetTypeId(string betTypeId, string defaultValue);

        string GetParentIdByBetTypeId(string betTypeId, string languageCode, string defaultValue);

        string GetBetTypeName(string betTypeId, string languageCode);

        string GetBetTypeName(string betTypeId, string languageCode, long refNo);

        string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo);

        string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId);

        string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue);

        string GetLeagueName(string leagueId);

        string GetLeagueName(string leagueId, string languageCode);

        string GetLeagueName(string leagueId, string languageCode, string defaultValue);

        string GetSportName(string sportTypeId, string languageCode);

        string GetSportName(string sportTypeId, string languageCode, string defaultValue);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId);

        string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        string GetKenoBettype(string kenoBetTypeId, string languageCode);

        string GetKenoBettype(string kenoBetTypeId, string languageCode, string defaultValue);

        string GetTeamName(string teamId);

        string GetTeamName(string teamId, string languageCode);

        string GetTeamName(string teamId, string languageCode, string defaultValue);

        string GetTeamName(string teamId, string languageCode, int sportTypeId);

        string GetCountryName(string contryCode, string languageCode, string defaultValue);

        string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue);
    }
}