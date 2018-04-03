namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using System;
    using global::RemotingRef;

    public class RefDataHelper : IRefDataHelper
    {
        public string GetBetTypeName(string betTypeId, string language)
        {
            return RefData.GetBetTypeName(betTypeId, language, null);
        }

        public string GetBetTypeName(string betTypeId, string language, long refNo)
        {
            return RefData.GetBetTypeName(betTypeId, language, refNo, null);
        }

        public string GetBetTypeName(string betTypeId, string language, string resourceId)
        {
            return RefData.GetBetTypeName(betTypeId, language, null, ResourceId: resourceId);
        }

        public string GetBetTypeName(string betTypeId, string language, string resourceId, long refNo)
        {
            return RefData.GetBetTypeName(betTypeId, language, null, resourceId, refNo);
        }

        public string GetCasinoBetItem(string itemId, string language)
        {
            return RefData.GetCasinoBetItem(itemId, language, null);
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string language)
        {
            return RefData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, language);
        }

        public string GetCountryName(string countryCode, string language)
        {
            return RefData.GetCountryName(countryCode, language, null);
        }

        public string GetFastMarkets225Result(string resultId, string language)
        {
            return RefData.GetFastMarkets225Result(resultId, language);
        }

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string language)
        {
            return RefData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, language);
        }

        public string GetHorseTeamName(string codeId, string language)
        {
            return RefData.GetHorseTeamName(codeId, language, null);
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string language, out string kenoBetTypeId)
        {
            return RefData.GetKenoBetChoice(kenoBetChoiceId, language, null, out kenoBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string language)
        {
            return RefData.GetKenoBettype(kenoBetTypeId, language, null);
        }

        public string GetLeagueName(string leagueId, string language)
        {
            return RefData.GetLeagueName(leagueId, language, null);
        }

#pragma warning disable S3717 // Track use of "NotImplementedException"

        public string GetLeagueName3rd(string league3rdId, string language)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }

#pragma warning restore S3717 // Track use of "NotImplementedException"

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string language, out string lotteryBetTypeId)
        {
            return RefData.GetLotteryBetChoice(lotteryBetChoiceId, language, null, out lotteryBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string language)
        {
            return RefData.GetLotteryBettype(lotteryBetTypeId, language, null);
        }

        public string GetOddsTypeName(string oddsTypeId, string language)
        {
            return RefData.GetOddsTypeName(oddsTypeId, language, null);
        }

        public string GetParentIdByBetTypeId(string betTypeId)
        {
            return RefData.GetParentIdByBetTypeId(betTypeId, null);
        }

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string language)
        {
            return RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, language, null);
        }

        public string GetRouletteBetItem(string itemId, string language)
        {
            return RefData.GetRouletteBetItem(itemId, language, null);
        }

        public string GetSportName(string sportTypeId, string language)
        {
            return RefData.GetSportName(sportTypeId, language, null);
        }

        public string GetTeamName(string teamId, string language)
        {
            return RefData.GetTeamName(teamId, language, null);
        }

#pragma warning disable S3717 // Track use of "NotImplementedException"

        public string GetTeamName3rd(string team3rdId, string language)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }

#pragma warning restore S3717 // Track use of "NotImplementedException"
    }
}