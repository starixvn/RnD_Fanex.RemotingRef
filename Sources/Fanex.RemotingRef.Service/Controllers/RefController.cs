namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.Serialization;
    using global::RemotingRef;
    using Microsoft.AspNetCore.Mvc;
    using static global::RemotingRef.RefData;

    [Route("api/[controller]")]
    public class RefController : Controller
    {
        private const string DefaultLanguage = "en-US";
        private readonly IRefData _refData;
        private readonly ISerializer _serializer;

        public RefController(IRefData refData, ISerializer serializer)
        {
            _refData = refData;
            _serializer = serializer;
        }

        [Route("BetTypeName")]
        public IActionResult BetTypeName(string betTypeId, string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNames));
        }

        [Route("BetTypeName")]
        public IActionResult BetTypeName(string betTypeId, string resourceId, string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, resourceId));
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNames));
        }

        [Route("BetTypeName")]
        public IActionResult BetTypeName(string betTypeId, string resourceId, long refNo, string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, resourceId, refNo));
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNames));
        }

        [Route("SportName")]
        public IActionResult SportName(string sportTypeId, string[] languages)
        {
            var sportNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                sportNames.Add(language, _refData.GetSportName(sportTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(sportNames));
        }

        [Route("TeamName")]
        public IActionResult TeamName(string teamId, string[] languages)
        {
            var teamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                teamNames.Add(language, _refData.GetTeamName(teamId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(teamNames));
        }

        [Route("TeamName3rd")]
        public IActionResult TeamName3rd(string team3rdId, string[] languages)
        {
            var teamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                // TODO: Update latest RemotingRef
            }

            return new OkObjectResult(_serializer.Serialize(teamNames));
        }

        [Route("LeagueName")]
        public IActionResult LeagueName(string leagueId, string[] languages)
        {
            var leagueNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                leagueNames.Add(language, _refData.GetLeagueName(leagueId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(leagueNames));
        }

        [Route("LeagueName3rd")]
        public IActionResult LeagueName3rd(string leagueId, string[] languages)
        {
            var leagueNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                // TODO: Update latest RemotingRef
            }

            return new OkObjectResult(_serializer.Serialize(leagueNames));
        }

        [Route("CountryName")]
        public IActionResult CountryName(string countryId, string[] languages)
        {
            var countryNames = new Dictionary<string, string>();

            foreach (var lang in languages)
            {
                countryNames.Add(lang, _refData.GetCountryName(countryId, LanguageHelper.GenerateTaiwanLanguage(lang), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(countryNames));
        }

        [Route("OddTypeName")]
        public IActionResult OddTypeName(string oddsTypeId, string[] languages)
        {
            var oddsTypeNames = new Dictionary<string, string>();

            foreach (var lang in languages)
            {
                oddsTypeNames.Add(lang, _refData.GetOddsTypeName(oddsTypeId, LanguageHelper.GenerateTaiwanLanguage(lang), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(oddsTypeNames));
        }

        [Route("ResourceData")]
        public IActionResult ResourceData(string refId, string resourceId, string[] languages)
        {
            var resourceData = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                resourceData.Add(language, _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(resourceData));
        }

        [Route("ParentIdByBetTypeId")]
        public IActionResult ParentIdByBetTypeId(string betTypeId)
        {
            var parentId = new Dictionary<string, string>
            {
                { DefaultLanguage, _refData.GetParentIdByBetTypeId(betTypeId, string.Empty) }
            };

            return new OkObjectResult(_serializer.Serialize(parentId));
        }

        [Route("CombinationTypeCombo")]
        public IActionResult CombinationTypeComboParlay(string betTeam, string betCheck, int handicap1, int handicap2, string[] languages)
        {
            var combinationTypes = new Dictionary<string, string[]>();

            foreach (var language in languages)
            {
                combinationTypes.Add(language, _refData.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return new OkObjectResult(_serializer.Serialize(combinationTypes));
        }

        [Route("FuncIndexName")]
        public IActionResult FuncIndexName(string indexId, string[] languages)
        {
            var funcIndexNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                funcIndexNames.Add(language, _refData.GetFncIndexName(indexId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(funcIndexNames));
        }

        [Route("HorseTeamName")]
        public IActionResult HorseTeamName(string codeId, string[] languages)
        {
            var horseTeamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                horseTeamNames.Add(language, _refData.GetHorseTeamName(codeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(horseTeamNames));
        }

        [Route("CasinoBetItem")]
        public IActionResult CasinoBetItem(string itemId, string[] languages)
        {
            var casinoBetItems = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                casinoBetItems.Add(language, _refData.GetCasinoBetItem(itemId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(casinoBetItems));
        }

        [Route("RouletteBetItem")]
        public IActionResult RouletteBetItem(string itemId, string[] languages)
        {
            var rouletteBetItems = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                rouletteBetItems.Add(language, _refData.GetRouletteBetItem(itemId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(rouletteBetItems));
        }

        [Route("KenoBetChoice")]
        public IActionResult KenoBetChoice(string betChoiceId, string[] languages)
        {
            var kenoBetChoices = new Tuple<Dictionary<string, string>, Dictionary<string, string>>(new Dictionary<string, string>(), new Dictionary<string, string>());

            foreach (var language in languages)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(betChoiceId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, out string kenoBetTypeId);

                kenoBetChoices.Item1.Add(language, kenoBetChoice);
                kenoBetChoices.Item2.Add(betChoiceId, kenoBetTypeId);
            }

            return new OkObjectResult(_serializer.Serialize(kenoBetChoices));
        }

        [Route("KenoBetType")]
        public IActionResult KenoBetType(string betTypeId, string[] languages)
        {
            var kenoBetTypes = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                kenoBetTypes.Add(language, _refData.GetKenoBettype(betTypeId, LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
            }

            return new OkObjectResult(_serializer.Serialize(kenoBetTypes));
        }

        [Route("FastMarketsChoice")]
        public IActionResult FastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string[] languages)
        {
            var fastMarketsChoices = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                fastMarketsChoices.Add(language, _refData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return new OkObjectResult(_serializer.Serialize(fastMarketsChoices));
        }

        [Route("FastMarkets225Result")]
        public IActionResult FastMarkets225Result(string resultId, string[] languages)
        {
            var fastMarkets225Results = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                fastMarkets225Results.Add(language, _refData.GetFastMarkets225Result(resultId, LanguageHelper.GenerateTaiwanLanguage(language)));
            }

            return new OkObjectResult(_serializer.Serialize(fastMarkets225Results));
        }

        [Route("ExchangeData")]
        public IActionResult ExchangeData(int exchangeId)
        {
            var exchangeData = new Dictionary<string, ExchangeData>
            {
                { "en-US", _refData.GetExchangeData(exchangeId) }
            };

            return new OkObjectResult(_serializer.Serialize(exchangeData));
        }

        [Route("LotteryBetChoice")]
        public IActionResult LotteryBetChoice(string betChoiceId, string[] languages)
        {
            var lotteryBetChoices = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                // TODO: Update latest RemotingRef
            }

            return new OkObjectResult(_serializer.Serialize(lotteryBetChoices));
        }

        [Route("LotteryBetTypeName")]
        public IActionResult LotteryBetTypeName(string betTypeId, string[] languages)
        {
            var lotteryBetTypes = new Dictionary<string, string>();

            foreach (var lang in languages)
            {
                // TODO: Update latest RemotingRef
            }

            return new OkObjectResult(_serializer.Serialize(lotteryBetTypes));
        }
    }
}