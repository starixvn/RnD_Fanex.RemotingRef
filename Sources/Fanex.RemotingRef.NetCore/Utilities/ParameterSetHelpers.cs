namespace Fanex.RemotingRef.NetCore.Utilities
{
    using System;
    using Fanex.RemotingRef.NetCore.Models;

    internal static class ParameterSetHelpers
    {
        public static string GetBetTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "betTypeId";

            var a= parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
            return a;
        }

        public static string GetResourceId(this ParameterSet parameterSet)
        {
            const string parameterName = "resourceId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static long? GetRefNo(this ParameterSet parameterSet)
        {
            const string parameterName = "refNo";

            if (parameterSet.ContainsKey(parameterName) && parameterSet[parameterName] != null)
            {
                return Convert.ToInt64(parameterSet[parameterName]);
            }

            return null;
        }

        public static string GetSportTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "sportTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetTeamId(this ParameterSet parameterSet)
        {
            const string parameterName = "teamId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetTeam3rdId(this ParameterSet parameterSet)
        {
            const string parameterName = "team3rdId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetLeagueId(this ParameterSet parameterSet)
        {
            const string parameterName = "leagueId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetLeague3rdId(this ParameterSet parameterSet)
        {
            const string parameterName = "league3rdId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetCountryCode(this ParameterSet parameterSet)
        {
            const string parameterName = "countryCode";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetOddsTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "oddsTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetRefId(this ParameterSet parameterSet)
        {
            const string parameterName = "refId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetBetTeam(this ParameterSet parameterSet)
        {
            const string parameterName = "betTeam";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetBetCheck(this ParameterSet parameterSet)
        {
            const string parameterName = "betCheck";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static int GetHandicap1(this ParameterSet parameterSet)
        {
            const string parameterName = "handicap1";

            return parameterSet.ContainsKey(parameterName) ? Convert.ToInt32(parameterSet[parameterName]) : default;
        }

        public static int GetHandicap2(this ParameterSet parameterSet)
        {
            const string parameterName = "handicap2";

            return parameterSet.ContainsKey(parameterName) ? Convert.ToInt32(parameterSet[parameterName]) : default;
        }

        public static string GetCodeId(this ParameterSet parameterSet)
        {
            const string parameterName = "codeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetItemId(this ParameterSet parameterSet)
        {
            const string parameterName = "itemId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetKenoBetChoiceId(this ParameterSet parameterSet)
        {
            const string parameterName = "kenoBetChoiceId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetKenoBetTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "kenoBetTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetResultId(this ParameterSet parameterSet)
        {
            const string parameterName = "resultId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetLotteryBetChoiceId(this ParameterSet parameterSet)
        {
            const string parameterName = "lotteryBetChoiceId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetLotteryBetTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "lotteryBetTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetBetChoiceId(this ParameterSet parameterSet)
        {
            const string parameterName = "betChoiceId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetMatchCode(this ParameterSet parameterSet)
        {
            const string parameterName = "matchCode";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static int GetSelectionId(this ParameterSet parameterSet)
        {
            const string parameterName = "selectionId";

            return parameterSet.ContainsKey(parameterName) ? Convert.ToInt32(parameterSet[parameterName]) : default;
        }

        public static string GetTableGamesBetChoiceId(this ParameterSet parameterSet)
        {
            const string parameterName = "TableGamesBetChoiceId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }

        public static string GetTableGamesBetTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "tableGamesBetTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName].ToString() : null;
        }
    }
}