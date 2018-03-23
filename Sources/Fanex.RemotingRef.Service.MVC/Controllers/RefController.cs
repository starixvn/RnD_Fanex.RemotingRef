namespace Fanex.RemotingRef.Service.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using global::RemotingRef;

    public class RefController : ApiController
    {
        private readonly IRefDataHelper _refData = new RefDataHelper();

        public RefController()
            : this(null)
        {
        }

        public RefController(IRefDataHelper refData)
        {
            _refData = refData ?? _refData;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeName(string betTypeId, [FromUri] string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNo(string betTypeId, long refNo, [FromUri] string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), refNo, string.Empty));
            }

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithResourceId(string betTypeId, string resourceId, [FromUri]string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, resourceId));
            }

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNoAndResourceId(string betTypeId, string resourceId, long refNo, [FromUri]string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, resourceId, refNo));
            }

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> SportName(string sportTypeId, [FromUri]string[] languages)
        {
            var sportNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                sportNames.Add(language, _refData.GetSportName(sportTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return sportNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName(string teamId, [FromUri]string[] languages)
        {
            var teamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                teamNames.Add(language, _refData.GetTeamName(teamId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName3rd(string team3rdId, [FromUri]string[] languages)
        {
            var teamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                teamNames.Add(language, _refData.GetTeamName3rd(team3rdId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName(string leagueId, [FromUri]string[] languages)
        {
            var leagueNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                leagueNames.Add(language, _refData.GetLeagueName(leagueId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return leagueNames;
        }

        public Dictionary<string, string> LeagueName3rd(string league3rdId, [FromUri]string[] languages)
        {
            var league3rdNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                // TODO: Update latest RemotingRef
                league3rdNames.Add(language, _refData.GetLeagueName3rd(league3rdId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return league3rdNames;
        }

        [HttpGet]
        public Dictionary<string, string> ResourceData(string refId, string resourceId, [FromUri]string[] languages)
        {
            var resourceData = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                resourceData.Add(
                    language,
                    _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return resourceData;
        }

        [HttpGet]
        public Dictionary<string, string> CountryName(string countryCode, [FromUri]string[] languages)
        {
            var countryNames = new Dictionary<string, string>();

            foreach (var lang in languages)
            {
                countryNames.Add(lang, _refData.GetCountryName(countryCode, LanguageHelper.GenerateTaiwanLanguage(lang), string.Empty));
            }

            return countryNames;
        }

        [HttpGet]
        public Dictionary<string, string> OddsTypeName(string oddsTypeId, [FromUri]string[] languages)
        {
            var oddsTypeNames = new Dictionary<string, string>();

            foreach (var lang in languages)
            {
                oddsTypeNames.Add(lang, _refData.GetOddsTypeName(oddsTypeId, LanguageHelper.GenerateTaiwanLanguage(lang), string.Empty));
            }

            return oddsTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> ParentIdByBetTypeId(string betTypeId)
        {
            var parentId = new Dictionary<string, string>
            {
                { LanguageHelper.DefaultLanguage, _refData.GetParentIdByBetTypeId(betTypeId, string.Empty) }
            };

            return parentId;
        }

        [HttpGet]
        public Dictionary<string, string[]> CombinationTypeComboParlay(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            [FromUri]string[] languages)
        {
            var combinationTypes = new Dictionary<string, string[]>();

            foreach (var language in languages)
            {
                combinationTypes.Add(language, _refData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return combinationTypes;
        }

        [HttpGet]
        public Dictionary<string, string> HorseTeamName(string codeId, [FromUri]string[] languages)
        {
            var horseTeamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                horseTeamNames.Add(language, _refData.GetHorseTeamName(codeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return horseTeamNames;
        }

        [HttpGet]
        public Dictionary<string, string> CasinoBetItem(string itemId, [FromUri]string[] languages)
        {
            var casinoBetItems = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                casinoBetItems.Add(language, _refData.GetCasinoBetItem(itemId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return casinoBetItems;
        }

        [HttpGet]
        public Dictionary<string, string> RouletteBetItem(string itemId, [FromUri]string[] languages)
        {
            var rouletteBetItems = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                rouletteBetItems.Add(language, _refData.GetRouletteBetItem(itemId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return rouletteBetItems;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> KenoBetChoice(string kenoBetChoiceId, [FromUri]string[] languages)
        {
            var kenoBetChoices = new Dictionary<string, Tuple<string, string>>();

            foreach (var language in languages)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(kenoBetChoiceId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, out string kenoBetTypeId);

                kenoBetChoices.Add(language, new Tuple<string, string>(kenoBetChoice, kenoBetTypeId));
            }

            return kenoBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> KenoBetType(string kenoBetTypeId, [FromUri]string[] languages)
        {
            var kenoBetTypes = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                kenoBetTypes.Add(language, _refData.GetKenoBetType(kenoBetTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return kenoBetTypes;
        }

        [HttpGet]
        public Dictionary<string, string> FastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, [FromUri]string[] languages)
        {
            var fastMarketsChoices = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                fastMarketsChoices.Add(language, _refData.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return fastMarketsChoices;
        }

        [HttpGet]
        public Dictionary<string, string> FastMarkets225Result(string resultId, [FromUri]string[] languages)
        {
            var fastMarkets225Results = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                fastMarkets225Results.Add(language, _refData.GetFastMarkets225Result(resultId, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return fastMarkets225Results;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> LotteryBetChoice(string betChoiceId, [FromUri]string[] languages)
        {
            var lotteryBetChoices = new Dictionary<string, Tuple<string, string>>();

            foreach (var language in languages)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(betChoiceId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, out string lotteryBetTypeId);

                lotteryBetChoices.Add(language, new Tuple<string, string>(lotteryBetChoice, lotteryBetTypeId));
            }

            return lotteryBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> LotteryBetType(string betTypeId, [FromUri]string[] languages)
        {
            var lotteryBetTypes = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                lotteryBetTypes.Add(language, _refData.GetLotteryBetType(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return lotteryBetTypes;
        }
    }
}