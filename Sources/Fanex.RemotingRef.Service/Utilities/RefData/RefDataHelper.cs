namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using System;
    using global::RemotingRef;

    public class RefDataHelper : IRefDataHelper
    {
        public string GetBetTypeName(string betTypeId, string languageCode)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, null);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, refNo, null);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string resourceId)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, null, ResourceId: resourceId);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string resourceId, long refNo)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, null, resourceId, refNo);
        }

        public string GetCasinoBetItem(string itemId, string languageCode)
        {
            return RefData.GetCasinoBetItem(itemId, languageCode, null);
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string languageCode)
        {
            return RefData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, languageCode);
        }

        public string GetCountryName(string countryCode, string languageCode)
        {
            return RefData.GetCountryName(countryCode, languageCode, null);
        }

        public string GetFastMarkets225Result(string resultId, string languageCode)
        {
            return RefData.GetFastMarkets225Result(resultId, languageCode);
        }

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode)
        {
            return RefData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, languageCode);
        }

        public string GetHorseTeamName(string codeId, string languageCode)
        {
            return RefData.GetHorseTeamName(codeId, languageCode, null);
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId)
        {
            return RefData.GetKenoBetChoice(kenoBetChoiceId, languageCode, null, out kenoBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string languageCode)
        {
            return RefData.GetKenoBettype(kenoBetTypeId, languageCode, null);
        }

        public string GetLeagueName(string leagueId, string languageCode)
        {
            return RefData.GetLeagueName(leagueId, languageCode, null);
        }

#pragma warning disable S3717 // Track use of "NotImplementedException"

        public string GetLeagueName3rd(string league3rdId, string languageCode)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }

#pragma warning restore S3717 // Track use of "NotImplementedException"

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, out string lotteryBetTypeId)
        {
            return RefData.GetLotteryBetChoice(lotteryBetChoiceId, languageCode, null, out lotteryBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode)
        {
            return RefData.GetLotteryBettype(lotteryBetTypeId, languageCode, null);
        }

        public string GetOddsTypeName(string oddsTypeId, string languageCode)
        {
            return RefData.GetOddsTypeName(oddsTypeId, languageCode, null);
        }

        public string GetParentIdByBetTypeId(string betTypeId)
        {
            return RefData.GetParentIdByBetTypeId(betTypeId, null);
        }

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode)
        {
            return RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, null);
        }

        public string GetRouletteBetItem(string itemId, string languageCode)
        {
            return RefData.GetRouletteBetItem(itemId, languageCode, null);
        }

        public string GetSportName(string sportTypeId, string languageCode)
        {
            return RefData.GetSportName(sportTypeId, languageCode, null);
        }

        public string GetTeamName(string teamId, string languageCode)
        {
            return RefData.GetTeamName(teamId, languageCode, null);
        }

#pragma warning disable S3717 // Track use of "NotImplementedException"

        public string GetTeamName3rd(string team3rdId, string languageCode)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }

#pragma warning restore S3717 // Track use of "NotImplementedException"
    }
}