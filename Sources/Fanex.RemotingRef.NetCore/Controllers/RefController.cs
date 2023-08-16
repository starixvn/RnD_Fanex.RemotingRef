namespace Fanex.RemotingRef.NetCore.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Fanex.RemotingRef.NetCore.Delegates;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Fanex.RemotingRef.NetCore.Utilities.RefData;

    [ApiController]
    [Route("/api/[controller]")]
    public class RefController : ControllerBase
    {
        private readonly IRefDataHelper _refData = new RefDataHelper();

        [HttpPost("BetTypeName")]
        public TranslationSet BetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromBody] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetBetTypeName(
                    betTypeId,
                    resourceId,
                    refNo,
                    refLanguagCode),
                languages);        

        [HttpPost("SportName")]
        public TranslationSet SportName(string sportTypeId = null, string languages = "en-us")
                    => GetTranslationSet(
                        refLanguagCode => _refData.GetSportName(sportTypeId, refLanguagCode),
                        languages.Split(','));

        [HttpPost("TeamName")]
        public TranslationSet TeamName(string teamId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetTeamName(teamId, refLanguagCode),
                languages.Split(','));

        [HttpPost("TeamName3rd")]
        public TranslationSet TeamName3rd(string team3rdId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetTeamName3rd(team3rdId, refLanguagCode),
                languages.Split(','));

        [HttpPost("LeagueName")]
        public TranslationSet LeagueName(string leagueId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetLeagueName(leagueId, refLanguagCode),
                languages.Split(','));

        [HttpPost("LeagueName3rd")]
        public TranslationSet LeagueName3rd(string league3rdId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetLeagueName3rd(league3rdId, refLanguagCode),
                languages.Split(','));

        [HttpPost("CountryName")]
        public TranslationSet CountryName(string countryCode = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetCountryName(countryCode, refLanguagCode),
                languages.Split(','));

        [HttpPost("OddsTypeName")]
        public TranslationSet OddsTypeName(string oddsTypeId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetOddsTypeName(oddsTypeId, refLanguagCode),
                languages.Split(','));

        [HttpPost("ResourceData")]
        public TranslationSet ResourceData(string refId = null, string resourceId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, refLanguagCode),
                languages.Split(','));

        [HttpPost("ParentIdByBetTypeId")]
        public TranslationSet ParentIdByBetTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetParentIdByBetTypeId(betTypeId));

        [HttpPost("CombinationTypeComboParlay")]
        public TranslationSet CombinationTypeComboParlay(
            string betTeam = null,
            string betCheck = null,
            int handicap1 = 0,
            int handicap2 = 0,
            [FromBody] string languages = "en-us")
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
                languages.Split(','),
                VerifyComboList);
        }

        [HttpPost("HorseTeamName")]
        public TranslationSet HorseTeamName(string codeId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetHorseTeamName(codeId, refLanguagCode),
                languages.Split(','));

        [HttpPost("CasinoBetItem")]
        public TranslationSet CasinoBetItem(string itemId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetCasinoBetItem(itemId, refLanguagCode),
                languages.Split(','));

        [HttpPost("RouletteBetItem")]
        public TranslationSet RouletteBetItem(string itemId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetRouletteBetItem(itemId, refLanguagCode),
                languages.Split(','));

        [HttpPost("KenoBetChoice")]
        public TranslationSet KenoBetChoice(string kenoBetChoiceId = null, string languages = "en-us")
        {
            string GetKenoBetChoiceAndType(string refLanguageCode)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(
                    kenoBetChoiceId,
                    refLanguageCode);

                return kenoBetChoice;
            }

            return GetTranslationSet(
                GetKenoBetChoiceAndType,
                languages.Split(','),
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost("KenoBetType")]
        public TranslationSet KenoBetType(string kenoBetTypeId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetKenoBetType(kenoBetTypeId, refLanguagCode),
                languages.Split(','));

        [HttpPost("FastMarketsChoice")]
        public TranslationSet FastMarketsChoice(
             string betTypeId = null,
             string betTeam = null,
             int handicap1 = 0,
             int handicap2 = 0,
             [FromBody] string languages = "en-us")
             => GetTranslationSet(
                 refLanguagCode => _refData.GetFastMarketsChoice(
                     betTypeId,
                     betTeam,
                     handicap1,
                     handicap2,
                     refLanguagCode),
                 languages.Split(','));

        [HttpPost("FastMarketsResult")]
        public TranslationSet FastMarketsResult(string resultId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetFastMarketsResult(resultId, refLanguagCode),
                languages.Split(','));

        [HttpPost("FastMarkets225Result")]
        public TranslationSet FastMarkets225Result(string resultId = null, string languages = "en-us")
             => GetTranslationSet(
                 refLanguagCode => _refData.GetFastMarkets225Result(resultId, refLanguagCode),
                 languages.Split(','));

        [HttpPost("LotteryBetChoice")]
        public TranslationSet LotteryBetChoice(
            string lotteryBetChoiceId = null,
            string betChoiceId = null,
            [FromBody] string languages = "en-us")
        {
            string GetLotteryBetChoiceAndType(string refLanguageCode)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(
                    lotteryBetChoiceId ?? betChoiceId,
                    refLanguageCode);

                return lotteryBetChoice;
            }

            return GetTranslationSet(
                GetLotteryBetChoiceAndType,
                languages.Split(','),
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost("LotteryBetType")]
        public TranslationSet LotteryBetType(
             string lotteryBetTypeId = null,
             string betTypeId = null,
             [FromBody] string languages = "en-us")
             => GetTranslationSet(
                 refLanguagCode => _refData.GetLotteryBetType(lotteryBetTypeId ?? betTypeId, refLanguagCode),
                 languages.Split(','));

        [HttpPost("BetRadarMatchCodeFormat")]
        public TranslationSet BetRadarMatchCodeFormat(string matchCode = null, string languages = "en-us")
             => GetTranslationSet(
                 refLanguagCode => _refData.GetBetRadarMatchCodeFormat(matchCode, refLanguagCode),
                 languages.Split(','));

        [HttpPost("DisplayedTypeId")]
        public TranslationSet DisplayedTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetDisplayedTypeId(betTypeId));

        [HttpPost("DisplayedBetTypeName")]
        public TranslationSet DisplayedBetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromBody] string languages = "en-us")
        {
            var displayedBetTypeId = _refData.GetDisplayedTypeId(betTypeId);

            return GetTranslationSet(
                refLanguagCode => _refData.GetBetTypeName(
                    displayedBetTypeId,
                    resourceId,
                    refNo,
                    refLanguagCode),
                languages.Split(','));
        }

        [HttpPost("BetBuilderSelectionName")]
        public TranslationSet BetBuilderSelectionName(int selectionId = 0, string languages = "en-us")
        {
            return GetTranslationSet(
                refLanguageCode => _refData.GetBetBuilderSelectionName(selectionId, refLanguageCode),
                languages.Split(','));
        }

        [HttpPost("TableGamesBetChoice")]
        public TranslationSet TableGamesBetChoice(string tableGamesBetChoiceId = null, string languages = "en-us")
        {
            string GetTableGamesBetChoiceAndType(string refLanguageCode)
            {
                var tableGamesBetChoice = _refData.GetTableGamesBetChoice(
                    tableGamesBetChoiceId,
                    refLanguageCode);

                return tableGamesBetChoice;
            }

            return GetTranslationSet(
                GetTableGamesBetChoiceAndType,
                languages.Split(','),
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost("TableGamesBetType")]
        public TranslationSet TableGamesBetType(string tableGamesBetTypeId = null, string languages = "en-us")
            => GetTranslationSet(
                refLanguagCode => _refData.GetTableGamesBetType(tableGamesBetTypeId, refLanguagCode),
                languages.Split(','));

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
                var resourceInSpecificLanguage = translationGetter(standardLanguageCode);

                if (verifier(resourceInSpecificLanguage))
                {
                    translationSet.Add(standardLanguageCode, resourceInSpecificLanguage);
                }
            }

            return translationSet.Count > 0 ? translationSet : null;
        }
    }
}