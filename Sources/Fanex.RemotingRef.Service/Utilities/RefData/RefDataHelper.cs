namespace Fanex.RemotingRef.Service.Utilities.RefData
{
    using global::RemotingRef;

    public class RefDataHelper : IRefDataHelper
    {
        /// <summary>
        /// We use NULL for default value cannot get from ONElab RemotingRef,
        /// because if we don't specific default name ONElab RemotingRef will return empty string,
        /// and client will cache empty string, so we use NULL to make client dont cache it.
        /// </summary>
        private const string DefaultValue = null;

        public string GetBetTypeName(
            string betTypeId,
            string resourceId,
            long? refNo,
            string language)
        {
            if (resourceId == null && refNo == null)
            {
                return RefData.GetBetTypeName(betTypeId, language, DefaultValue);
            }

            if (resourceId == null)
            {
                return RefData.GetBetTypeName(
                    betTypeId,
                    language,
                    refNo.Value,
                    DefaultValue);
            }

            if (refNo == null)
            {
                return RefData.GetBetTypeName(
                    betTypeId,
                    language,
                    DefaultValue,
                    resourceId);
            }

            return RefData.GetBetTypeName(
                betTypeId,
                language,
                DefaultValue,
                resourceId,
                refNo.Value);
        }

        public string GetSportName(string sportTypeId, string language)
            => RefData.GetSportName(sportTypeId, language, DefaultValue);

        public string GetTeamName(string teamId, string language)
            => RefData.GetTeamName(teamId, language, DefaultValue);

        public string GetTeamName3rd(string team3rdId, string language)
            => RefData.GetTeamName3rd(team3rdId, language, DefaultValue);

        public string GetLeagueName(string leagueId, string language)
            => RefData.GetLeagueName(leagueId, language, DefaultValue);

        public string GetLeagueName3rd(string league3rdId, string language)
            => RefData.GetLeagueName3rd(league3rdId, language, DefaultValue);

        public string GetCountryName(string countryCode, string language)
            => RefData.GetCountryName(countryCode, language, DefaultValue);

        public string GetOddsTypeName(string oddsTypeId, string language)
            => RefData.GetOddsTypeName(oddsTypeId, language, DefaultValue);

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string language)
            => RefData.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                language,
                DefaultValue);

        public string GetParentIdByBetTypeId(string betTypeId)
            => RefData.GetParentIdByBetTypeId(betTypeId, DefaultValue);

        public string[] GetCombinationTypeCombo(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            string language)
            => RefData.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                language);

        public string GetHorseTeamName(string codeId, string language)
            => RefData.GetHorseTeamName(codeId, language, DefaultValue);

        public string GetCasinoBetItem(string itemId, string language)
            => RefData.GetCasinoBetItem(itemId, language, DefaultValue);

        public string GetRouletteBetItem(string itemId, string language)
            => RefData.GetRouletteBetItem(itemId, language, DefaultValue);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string language, out string kenoBetTypeId)
            => RefData.GetKenoBetChoice(
                kenoBetChoiceId,
                language,
                DefaultValue,
                out kenoBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string language)
            => RefData.GetKenoBettype(kenoBetTypeId, language, DefaultValue);

        public string GetFastMarketsChoice(
            string betTypeId,
            string betTeam,
            int handicap1,
            int handicap2,
            string language)
            => RefData.GetFastMarketsChoise(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                language);

        public string GetFastMarketsResult(string resultId, string language)
            => RefData.GetFastMarketsResult(resultId, language);

        public string GetFastMarkets225Result(string resultId, string language)
            => RefData.GetFastMarkets225Result(resultId, language);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string language, out string lotteryBetTypeId)
            => RefData.GetLotteryBetChoice(
                lotteryBetChoiceId,
                language,
                DefaultValue,
                out lotteryBetTypeId);

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string language)
            => RefData.GetLotteryBettype(lotteryBetTypeId, language, DefaultValue);

        public string GetBetRadarMatchCodeFormat(string matchCode, string language)
            => RefData.GetBetRadarMatchCodeFormat(matchCode, language);

        public string GetDisplayedTypeId(string betTypeId)
            => RefData.GetDisplayedTypeId(betTypeId);
    }
}