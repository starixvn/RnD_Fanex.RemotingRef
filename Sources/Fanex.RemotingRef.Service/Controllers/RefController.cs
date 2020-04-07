namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Delegates;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;

    public class RefController : ApiController
    {
        private readonly IRefDataHelper _refData;

        public RefController()
            : this(null)
        {
        }

        public RefController(IRefDataHelper refData)
            => _refData = refData ?? new RefDataHelper();

        [HttpGet]
        public TranslationSet BetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetBetTypeName(
                    betTypeId,
                    resourceId,
                    refNo,
                    refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet SportName(string sportTypeId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetSportName(sportTypeId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet TeamName(string teamId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetTeamName(teamId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet TeamName3rd(string team3rdId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetTeamName3rd(team3rdId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet LeagueName(string leagueId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetLeagueName(leagueId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet LeagueName3rd(string league3rdId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetLeagueName3rd(league3rdId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet CountryName(string countryCode = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetCountryName(countryCode, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet OddsTypeName(string oddsTypeId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetOddsTypeName(oddsTypeId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet ResourceData(string refId = null, string resourceId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet ParentIdByBetTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetParentIdByBetTypeId(betTypeId));

        [HttpGet]
        public TranslationSet CombinationTypeComboParlay(
            string betTeam = null,
            string betCheck = null,
            int handicap1 = 0,
            int handicap2 = 0,
            [FromUri]string[] languages = null)
        {
            bool VerifyComboList(string[] combos)
                => combos != null && combos.Any(combo => !string.IsNullOrEmpty(combo));

            return GetTranslationSet(
                refLanguageCode => _refData.GetCombinationTypeCombo(
                    betTeam,
                    betCheck,
                    handicap1,
                    handicap2,
                    refLanguageCode),
                languages,
                VerifyComboList);
        }

        [HttpGet]
        public TranslationSet HorseTeamName(string codeId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetHorseTeamName(codeId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet CasinoBetItem(string itemId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetCasinoBetItem(itemId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet RouletteBetItem(string itemId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetRouletteBetItem(itemId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet KenoBetChoice(string kenoBetChoiceId = null, [FromUri]string[] languages = null)
        {
            Tuple<string, string> GetKenoBetChoiceAndType(string refLanguageCode)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(
                    kenoBetChoiceId,
                    refLanguageCode,
                    out string kenoBetTypeId);

                if (string.IsNullOrEmpty(kenoBetChoice) && string.IsNullOrEmpty(kenoBetTypeId))
                {
                    return null;
                }

                return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
            }

            return GetTranslationSet(
                GetKenoBetChoiceAndType,
                languages,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpGet]
        public TranslationSet KenoBetType(string kenoBetTypeId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetKenoBetType(kenoBetTypeId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet FastMarketsChoice(
            string betTypeId = null,
            string betTeam = null,
            int handicap1 = 0,
            int handicap2 = 0,
            [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetFastMarketsChoice(
                    betTypeId,
                    betTeam,
                    handicap1,
                    handicap2,
                    refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet FastMarketsResult(string resultId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetFastMarketsResult(resultId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet FastMarkets225Result(string resultId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetFastMarkets225Result(resultId, refLanguagCode),
                languages);

        // We accept both `lotteryBetChoiceId` and `betChoiceId` to give Fanex.RemotingRef-1.3.0 or higher verion ability to standardize
        // parameters in its requests, as well as let Fanex.RemotingRef-1.2.0 or lower version be happy with new Fanex.RemotingRef.Service.
        // We are planning to cleanup this mess once all sites upgrade to the latest version of Fanex.RemotingRef.
        [HttpGet]
        public TranslationSet LotteryBetChoice(
            string lotteryBetChoiceId = null,
            string betChoiceId = null,
            [FromUri]string[] languages = null)
        {
            Tuple<string, string> GetLotteryBetChoiceAndType(string refLanguageCode)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(
                    lotteryBetChoiceId ?? betChoiceId,
                    refLanguageCode,
                    out string lotteryBetTypeId);

                if (string.IsNullOrEmpty(lotteryBetChoice) && string.IsNullOrEmpty(lotteryBetTypeId))
                {
                    return null;
                }

                return new Tuple<string, string>(lotteryBetChoice, lotteryBetTypeId);
            }

            return GetTranslationSet(
                GetLotteryBetChoiceAndType,
                languages,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        // We accept both `lotteryBetTypeId` and `betTypeId` to give Fanex.RemotingRef-1.3.0 or higher version ability to standardize
        // parameters in its requests, as well as let Fanex.RemotingRef-1.2.0 or lower version be happy with new Fanex.RemotingRef.Service.
        // We are planning to cleanup this mess once all sites upgrade to the latest version of Fanex.RemotingRef.
        [HttpGet]
        public TranslationSet LotteryBetType(
            string lotteryBetTypeId = null,
            string betTypeId = null,
            [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetLotteryBetType(lotteryBetTypeId ?? betTypeId, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet BetRadarMatchCodeFormat(string matchCode = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetBetRadarMatchCodeFormat(matchCode, refLanguagCode),
                languages);

        [HttpGet]
        public TranslationSet DisplayedTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetDisplayedTypeId(betTypeId));

        [HttpGet]
        public TranslationSet DisplayedBetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromUri]string[] languages = null)
        {
            var displayedBetTypeId = _refData.GetDisplayedTypeId(betTypeId);

            return GetTranslationSet(
                refLanguagCode => _refData.GetBetTypeName(
                    displayedBetTypeId,
                    resourceId,
                    refNo,
                    refLanguagCode),
                languages);
        }

        [HttpGet]
        public TranslationSet BetBuilderSelectionName(int selectionId = 0, [FromUri]string[] languages = null)
        {
            return GetTranslationSet(
                refLanguageCode => _refData.GetBetBuilderSelectionName(selectionId, refLanguageCode),
                languages);
        }

        [HttpGet]
        public TranslationSet TableGamesBetChoice(string tableGamesBetChoiceId = null, [FromUri]string[] languages = null)
        {
            Tuple<string, string> GetTableGamesBetChoiceAndType(string refLanguageCode)
            {
                var tableGamesBetChoice = _refData.GetTableGamesBetChoice(
                    tableGamesBetChoiceId,
                    refLanguageCode,
                    out string tableGamesBetTypeId);

                if (string.IsNullOrEmpty(tableGamesBetChoice) && string.IsNullOrEmpty(tableGamesBetTypeId))
                {
                    return null;
                }

                return new Tuple<string, string>(tableGamesBetChoice, tableGamesBetTypeId);
            }

            return GetTranslationSet(
                GetTableGamesBetChoiceAndType,
                languages,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpGet]
        public TranslationSet TableGamesBetType(string tableGamesBetTypeId = null, [FromUri]string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetTableGamesBetType(tableGamesBetTypeId, refLanguagCode),
                languages);

        private static TranslationSet GetNeutralText(GetNeutralResouceDelegate<string> textGetter)
        {
            var text = textGetter();

            if (!string.IsNullOrEmpty(text))
            {
                return new TranslationSet
                {
                    [LanguageHelpers.DefaultLanguage] = text
                };
            }

            return default;
        }

        private static TranslationSet GetTranslationSet(GetTranslationDelegate<string> translatedTextGetter, string[] standardLanguageCodes)
            => GetTranslationSet(
                translatedTextGetter,
                standardLanguageCodes,
                text => !string.IsNullOrEmpty(text));

        private static TranslationSet GetTranslationSet<T>(
            GetTranslationDelegate<T> translationGetter,
            string[] standardLanguageCodes,
            VerifyTranslationDelegate<T> verifier)
        {
            var translationSet = new TranslationSet();

            foreach (var standardLanguageCode in standardLanguageCodes)
            {
                var resourceInSpecificLanguage = translationGetter(LanguageHelpers.ToRefLanguageCode(standardLanguageCode));

                if (verifier(resourceInSpecificLanguage))
                {
                    translationSet.Add(standardLanguageCode, resourceInSpecificLanguage);
                }
            }

            return translationSet.Count > 0 ? translationSet : null;
        }
    }
}