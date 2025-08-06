namespace Fanex.RemotingRef.NetCore.Controllers
{
    using Fanex.RemotingRef.NetCore.Delegates;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Fanex.RemotingRef.NetCore.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]")]
    public class RefController : Controller
    {
        private readonly IRefDataHelper _refData;
        private readonly ITeam3rdService _team3rdService;
        private readonly ILeague3rdService _league3rdService;

        public RefController(IRefDataHelper refData, ITeam3rdService team3rdService, ILeague3rdService league3rdService)
        => (_refData, _team3rdService, _league3rdService) = (refData, team3rdService, league3rdService);

        [HttpGet("BetTypeName")]
        public TranslationSet BetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetBetTypeName(
                    betTypeId,
                    resourceId,
                    refNo,
                    refLanguagCode),
                languages);

        [HttpGet("SportName")]
        public TranslationSet SportName(string sportTypeId = null, [FromQuery] string[] languages = null)
                    => GetTranslationSet(
                        refLanguagCode => _refData.GetSportName(sportTypeId, refLanguagCode),
                        languages);

        [HttpGet("TeamName")]
        public TranslationSet TeamName(string teamId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetTeamName(teamId, refLanguagCode),
                languages);

        [HttpGet("TeamName3rd")]
        public TranslationSet TeamName3rd(string team3rdId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _team3rdService.GetTeam3rdById(team3rdId, refLanguagCode),
                languages);

        [HttpGet("LeagueName")]
        public TranslationSet LeagueName(string leagueId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetLeagueName(leagueId, refLanguagCode),
                languages);

        [HttpGet("LeagueName3rd")]
        public TranslationSet LeagueName3rd(string league3rdId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _league3rdService.GetLeague3rdById(league3rdId, refLanguagCode),
                languages);

        [HttpGet("CountryName")]
        public TranslationSet CountryName(string countryCode = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetCountryName(countryCode, refLanguagCode),
                languages);

        [HttpGet("OddsTypeName")]
        public TranslationSet OddsTypeName(string oddsTypeId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetOddsTypeName(oddsTypeId, refLanguagCode),
                languages);

        [HttpGet("ResourceData")]
        public TranslationSet ResourceData(string refId = null, string resourceId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, refLanguagCode),
                languages);

        [HttpGet("ParentIdByBetTypeId")]
        public TranslationSet ParentIdByBetTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetParentIdByBetTypeId(betTypeId));

        [HttpGet("CombinationTypeComboParlay")]
        public TranslationSet CombinationTypeComboParlay(
            string betTeam = null,
            string betCheck = null,
            int handicap1 = 0,
            int handicap2 = 0,
            [FromQuery] string[] languages = null)
        {
            bool VerifyComboList(List<string> combos)
                => combos != null && combos.Exists(combo => !string.IsNullOrEmpty(combo));

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

        [HttpGet("HorseTeamName")]
        public TranslationSet HorseTeamName(string codeId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetHorseTeamName(codeId, refLanguagCode),
                languages);

        [HttpGet("CasinoBetItem")]
        public TranslationSet CasinoBetItem(string itemId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetCasinoBetItem(itemId, refLanguagCode),
                languages);

        [HttpGet("RouletteBetItem")]
        public TranslationSet RouletteBetItem(string itemId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetRouletteBetItem(itemId, refLanguagCode),
                languages);

        [HttpGet("KenoBetChoice")]
        public TranslationSet KenoBetChoice(string kenoBetChoiceId = null, [FromQuery] string[] languages = null)
        {
            (string betchoiceName, string bettypeId) GetKenoBetChoiceAndType(string refLanguageCode)
                => _refData.GetKenoBetChoice(kenoBetChoiceId, refLanguageCode);

            return GetTranslationSet(
                GetKenoBetChoiceAndType,
                languages,
                result => !string.IsNullOrEmpty(result.betchoiceName));
        }

        [HttpGet("KenoBetType")]
        public TranslationSet KenoBetType(string kenoBetTypeId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetKenoBetType(kenoBetTypeId, refLanguagCode),
                languages);

        [HttpGet("FastMarketsChoice")]
        public TranslationSet FastMarketsChoice(
             string betTypeId = null,
             string betTeam = null,
             int handicap1 = 0,
             int handicap2 = 0,
             [FromQuery] string[] languages = null)
             => GetTranslationSet(
                 refLanguagCode => _refData.GetFastMarketsChoice(
                     betTypeId,
                     betTeam,
                     handicap1,
                     handicap2,
                     refLanguagCode),
                 languages);

        [HttpGet("FastMarketsResult")]
        public TranslationSet FastMarketsResult(string resultId = null, [FromQuery] string[] languages = null)
            => GetTranslationSet(
                refLanguagCode => _refData.GetFastMarketsResult(resultId, refLanguagCode),
                languages);

        [HttpGet("FastMarkets225Result")]
        public TranslationSet FastMarkets225Result(string resultId = null, [FromQuery] string[] languages = null)
             => GetTranslationSet(
                 refLanguagCode => _refData.GetFastMarkets225Result(resultId, refLanguagCode),
                 languages);

        [HttpGet("LotteryBetChoice")]
        public TranslationSet LotteryBetChoice(
            string lotteryBetChoiceId = null,
            string betChoiceId = null,
            [FromQuery] string[] languages = null)
        {
            (string betchoiceName, string lotteryBettypeId) GetLotteryBetChoiceAndType(string refLanguageCode)
                => _refData.GetLotteryBetChoice(lotteryBetChoiceId ?? betChoiceId, refLanguageCode);

            return GetTranslationSet(
                GetLotteryBetChoiceAndType,
                languages,
                result => !string.IsNullOrEmpty(result.betchoiceName));
        }

        [HttpGet("LotteryBetType")]
        public TranslationSet LotteryBetType(
             string lotteryBetTypeId = null,
             string betTypeId = null,
             [FromQuery] string[] languages = null)
             => GetTranslationSet(
                 refLanguagCode => _refData.GetLotteryBetType(lotteryBetTypeId ?? betTypeId, refLanguagCode),
                 languages);

        [HttpGet("BetRadarMatchCodeFormat")]
        public TranslationSet BetRadarMatchCodeFormat(string matchCode = null, [FromQuery] string[] languages = null)
             => GetTranslationSet(
                 refLanguagCode => _refData.GetBetRadarMatchCodeFormat(matchCode, refLanguagCode),
                 languages);

        [HttpGet("DisplayedTypeId")]
        public TranslationSet DisplayedTypeId(string betTypeId = null)
            => GetNeutralText(() => _refData.GetDisplayedTypeId(betTypeId));

        [HttpGet("DisplayedBetTypeName")]
        public TranslationSet DisplayedBetTypeName(
            string betTypeId = null,
            string resourceId = null,
            long? refNo = null,
            [FromQuery] string[] languages = null)
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

        [HttpGet("BetBuilderSelectionName")]
        public TranslationSet BetBuilderSelectionName(int selectionId = 0, [FromQuery] string[] languages = null)
        {
            return GetTranslationSet(
                refLanguageCode => _refData.GetBetBuilderSelectionName(selectionId, refLanguageCode),
                languages);
        }

        [HttpGet("TableGamesBetChoice")]
        public TranslationSet TableGamesBetChoice(string tableGamesBetChoiceId = null, [FromQuery] string[] languages = null)
        {
            (string betchoiceName, string tableGamesBettypeId) GetTableGamesBetChoiceAndType(string refLanguageCode)
            => _refData.GetTableGamesBetChoice(tableGamesBetChoiceId, refLanguageCode);

            return GetTranslationSet(
                GetTableGamesBetChoiceAndType,
                languages,
                result => !string.IsNullOrEmpty(result.betchoiceName));
        }

        [HttpGet("TableGamesBetType")]
        public TranslationSet TableGamesBetType(string tableGamesBetTypeId = null, [FromQuery] string[] languages = null)
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

            return new TranslationSet();
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

            return translationSet;
        }
    }
}