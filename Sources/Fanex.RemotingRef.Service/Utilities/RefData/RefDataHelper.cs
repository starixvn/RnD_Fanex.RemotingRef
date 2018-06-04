namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using global::RemotingRef;

    public class RefDataHelper : IRefDataHelper
    {
        /// <summary>
        /// We use NULL for default value cannot get from ONElab RemotingRef,
        /// because if we don't specific default name ONElab RemotingRef will return empty string,
        /// and client will cache empty string, so we use NULL to make client dont cache it
        /// </summary>
        private const string DefaultName = null;

        public string GetBetTypeName(string betTypeId, string language)
            => RefData.GetBetTypeName(betTypeId, language, DefaultName);

        public string GetBetTypeName(string betTypeId, string language, long refNo)
            => RefData.GetBetTypeName(betTypeId, language, refNo, DefaultName);

        public string GetBetTypeName(string betTypeId, string language, string resourceId)
            => RefData.GetBetTypeName(betTypeId, language, DefaultName, ResourceId: resourceId);

        public string GetBetTypeName(string betTypeId, string language, string resourceId, long refNo)
            => RefData.GetBetTypeName(betTypeId, language, DefaultName, resourceId, refNo);

        public string GetCasinoBetItem(string itemId, string language)
            => RefData.GetCasinoBetItem(itemId, language, DefaultName);

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string language)
            => RefData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, language);

        public string GetCountryName(string countryCode, string language)
            => RefData.GetCountryName(countryCode, language, DefaultName);

        public string GetFastMarkets225Result(string resultId, string language)
            => RefData.GetFastMarkets225Result(resultId, language);

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string language)
            => RefData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, language);

        public string GetHorseTeamName(string codeId, string language)
            => RefData.GetHorseTeamName(codeId, language, DefaultName);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string language, out string kenoBetTypeId)
            => RefData.GetKenoBetChoice(kenoBetChoiceId, language, DefaultName, out kenoBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string language)
            => RefData.GetKenoBettype(kenoBetTypeId, language, DefaultName);

        public string GetLeagueName(string leagueId, string language)
            => RefData.GetLeagueName(leagueId, language, DefaultName);

        public string GetLeagueName3rd(string league3rdId, string language)
            => RefData.GetLeagueName3rd(league3rdId, language, DefaultName);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string language, out string lotteryBetTypeId)
            => RefData.GetLotteryBetChoice(lotteryBetChoiceId, language, DefaultName, out lotteryBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string language)
            => RefData.GetLotteryBettype(lotteryBetTypeId, language, DefaultName);

        public string GetOddsTypeName(string oddsTypeId, string language)
            => RefData.GetOddsTypeName(oddsTypeId, language, DefaultName);

        public string GetParentIdByBetTypeId(string betTypeId)
            => RefData.GetParentIdByBetTypeId(betTypeId, DefaultName);

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string language)
            => RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, language, DefaultName);

        public string GetRouletteBetItem(string itemId, string language)
            => RefData.GetRouletteBetItem(itemId, language, DefaultName);

        public string GetSportName(string sportTypeId, string language)
            => RefData.GetSportName(sportTypeId, language, DefaultName);

        public string GetTeamName(string teamId, string language)
            => RefData.GetTeamName(teamId, language, DefaultName);

        public string GetTeamName3rd(string team3rdId, string language)
            => RefData.GetTeamName3rd(team3rdId, language, DefaultName);

        public string GetBetRadarMatchCodeFormat(string matchCode, string language)
            => RefData.GetBetRadarMatchCodeFormat(matchCode, language);

        public string GetDisplayedTypeId(string betTypeId)
            => RefData.GetDisplayedTypeId(betTypeId);
    }
}