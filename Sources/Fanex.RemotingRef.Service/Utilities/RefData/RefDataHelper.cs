namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using System;
    using global::RemotingRef;

    public class RefDataHelper : IRefDataHelper
    {
        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, defaultValue);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, refNo, defaultValue);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, defaultValue, ResourceId: resourceId);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo)
        {
            return RefData.GetBetTypeName(betTypeId, languageCode, defaultValue, resourceId, refNo);
        }

        public string GetCasinoBetItem(string itemId, string languageCode, string defaultValue)
        {
            return RefData.GetCasinoBetItem(itemId, languageCode, defaultValue);
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string languageCode)
        {
            return RefData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, languageCode);
        }

        public string GetCountryName(string countryCode, string languageCode, string defaultValue)
        {
            return RefData.GetCountryName(countryCode, languageCode, defaultValue);
        }

        public string GetFastMarkets225Result(string resultId, string languageCode)
        {
            return RefData.GetFastMarkets225Result(resultId, languageCode);
        }

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode)
        {
            return RefData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, languageCode);
        }

        public string GetHorseTeamName(string codeId, string languageCode, string defaultValue)
        {
            return RefData.GetHorseTeamName(codeId, languageCode, defaultValue);
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId)
        {
            return RefData.GetKenoBetChoice(kenoBetChoiceId, languageCode, defaultValue, out kenoBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string languageCode, string defaultValue)
        {
            return RefData.GetKenoBettype(kenoBetTypeId, languageCode, defaultValue);
        }

        public string GetLeagueName(string leagueId, string languageCode, string defaultValue)
        {
            return RefData.GetLeagueName(leagueId, languageCode, defaultValue);
        }

        public string GetLeagueName3rd(string league3rdId, string languageCode, string defaultValue)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, string defaultValue, out string lotteryBetTypeId)
        {
            return RefData.GetLotteryBetChoice(lotteryBetChoiceId, languageCode, defaultValue, out lotteryBetTypeId);
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode, string defaultValue)
        {
            return RefData.GetLotteryBettype(lotteryBetTypeId, languageCode, defaultValue);
        }

        public string GetOddsTypeName(string oddsTypeId, string languageCode, string defaultValue)
        {
            return RefData.GetOddsTypeName(oddsTypeId, languageCode, defaultValue);
        }

        public string GetParentIdByBetTypeId(string betTypeId, string defaultValue)
        {
            return RefData.GetParentIdByBetTypeId(betTypeId, defaultValue);
        }

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue)
        {
            return RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, defaultValue);
        }

        public string GetRouletteBetItem(string itemId, string languageCode, string defaultValue)
        {
            return RefData.GetRouletteBetItem(itemId, languageCode, defaultValue);
        }

        public string GetSportName(string sportTypeId, string languageCode, string defaultValue)
        {
            return RefData.GetSportName(sportTypeId, languageCode, defaultValue);
        }

        public string GetTeamName(string teamId, string languageCode, string defaultValue)
        {
            return RefData.GetTeamName(teamId, languageCode, defaultValue);
        }

        public string GetTeamName3rd(string team3rdId, string languageCode, string defaultValue)
        {
            throw new NotImplementedException(); // TODO: Update latest remoting ref
        }
    }
}