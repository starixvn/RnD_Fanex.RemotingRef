namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.Serialization;
    using global::RemotingRef;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RefSyncController : Controller
    {
        private readonly IRefData _refData;
        private readonly ISerializer _serializer;

        public RefSyncController(IRefData refData, ISerializer serializer)
        {
            _refData = refData;
            _serializer = serializer;
        }

        [Route("BetTypeName")]
        [HttpPost]
        public IActionResult BetTypeName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var betTypeNameSet = new Dictionary<string, Dictionary<string, string>>();
            var parsedParameterSets = _serializer.Deserialize<List<Dictionary<string, object>>>(parameterSets);
            var parsedLanguages = _serializer.Deserialize<string[]>(languages);

            foreach (var parameterSet in parsedParameterSets)
            {
                var betTypeNames = GetBetTypeNames(parsedLanguages, parameterSet);

                betTypeNameSet.Add(parameterSet["betTypeId"].ToString(), betTypeNames);
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNameSet));
        }

        private Dictionary<string, string> GetBetTypeNames(string[] languages, Dictionary<string, object> parameterSet)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                if (parameterSet.Keys.Count == 1)
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(parameterSet["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("refNo"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        (long)parameterSet["refNo"],
                        string.Empty));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("resourceId"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        string.Empty,
                        parameterSet["resourceId"].ToString()));
                }
                else
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                       parameterSet["betTypeId"].ToString(),
                       LanguageHelper.GenerateTaiwanLanguage(language),
                       string.Empty,
                       parameterSet["resourceId"].ToString(),
                        (long)parameterSet["refNo"]));
                }
            }

            return betTypeNames;
        }

        [Route("SportName")]
        [HttpPost]
        public IActionResult SportName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var sportNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetSportName(parameter["sportTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "sportTypeId");

            return Ok(_serializer.Serialize(sportNameSet));
        }

        [Route("TeamName")]
        [HttpPost]
        public IActionResult TeamName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var teamNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetTeamName(parameter["teamId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "teamId");

            return Ok(_serializer.Serialize(teamNameSet));
        }

        [Route("LeagueName")]
        [HttpPost]
        public IActionResult LeagueName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var leagueNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetLeagueName(parameter["leagueId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "leagueId");

            return Ok(_serializer.Serialize(leagueNameSet));
        }

        [Route("CountryName")]
        [HttpPost]
        public IActionResult CountryName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var countryNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetCountryName(parameter["countryId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "countryId");

            return Ok(_serializer.Serialize(countryNameSet));
        }

        [Route("OddsTypeName")]
        [HttpPost]
        public IActionResult OddsTypeName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var oddsTypeNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetOddsTypeName(parameter["oddsTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "oddsTypeId");

            return Ok(_serializer.Serialize(oddsTypeNameSet));
        }

        [Route("ResourceData")]
        [HttpPost]
        public IActionResult ResourceData([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var resourceDataSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetResourceDataByRefIdAndResourceId(parameter["refId"].ToString(), parameter["resourceId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "refId");

            return Ok(_serializer.Serialize(resourceDataSet));
        }

        [Route("ParentIdByBetTypeId")]
        [HttpPost]
        public IActionResult ParentIdByBetTypeId([FromBody]byte[] parameterSets)
        {
            var parentIdSet = GetResultSet(
                parameterSets,
                null,
                (parameter, _) => _refData.GetParentIdByBetTypeId(parameter["betTypeId"].ToString(), string.Empty),
                "betTypeId");

            return Ok(_serializer.Serialize(parentIdSet));
        }

        [Route("CombinationTypeComboParlay")]
        [HttpPost]
        public IActionResult CombinationTypeComboParlay([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var combinationTypeSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetCombinationTypeCombo(
                        parameter["betTeam"].ToString(),
                        parameter["betCheck"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)),
                "betTeam");

            return Ok(_serializer.Serialize(combinationTypeSet));
        }

        [Route("HorseTeamName")]
        [HttpPost]
        public IActionResult HorseTeamName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var horseTeamNameSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetHorseTeamName(parameter["codeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "codeId");

            return Ok(_serializer.Serialize(horseTeamNameSet));
        }

        [Route("CasinoBetItem")]
        [HttpPost]
        public IActionResult CasinoBetItem([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var casinoBetItemSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetCasinoBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "itemId");

            return Ok(_serializer.Serialize(casinoBetItemSet));
        }

        [Route("RouletteBetItem")]
        [HttpPost]
        public IActionResult RouletteBetItem([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var rouletteBetItemSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetRouletteBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "itemId");

            return Ok(_serializer.Serialize(rouletteBetItemSet));
        }

        [Route("KenoBetType")]
        [HttpPost]
        public IActionResult KenoBetType([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var kenoBetTypeSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetKenoBettype(parameter["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty),
                "betTypeId");

            return Ok(_serializer.Serialize(kenoBetTypeSet));
        }

        [Route("KenoBetChoice")]
        [HttpPost]
        public IActionResult KenoBetChoice([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var kenoBetChoiceSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language) =>
                    {
                        var kenoBetChoice = _refData.GetKenoBetChoice(parameter["betChoiceId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty, out string kenoBetTypeId);

                        return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
                    },
                "betChoiceId");

            return Ok(_serializer.Serialize(kenoBetChoiceSet));
        }

        [Route("FastMarketsChoice")]
        [HttpPost]
        public IActionResult FastMarketsChoice([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var fastMarketChoiceSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetFastMarketsChoise(
                        parameter["betTypeId"].ToString(),
                        parameter["betTeam"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)),
                "betTypeId");

            return Ok(_serializer.Serialize(fastMarketChoiceSet));
        }

        [Route("FastMarkets225Result")]
        [HttpPost]
        public IActionResult FastMarkets225Result([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var fastMarkets225ResultSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language)
                    => _refData.GetFastMarkets225Result(parameter["resultId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)),
                "resultId");

            return Ok(_serializer.Serialize(fastMarkets225ResultSet));
        }

        [Route("LotteryBetChoice")]
        [HttpPost]
        public IActionResult LotteryBetChoice([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var fastMarkets225ResultSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language) => string.Empty, // TODO: Update latest RemotingRef
                "resultId");

            return Ok(_serializer.Serialize(fastMarkets225ResultSet));
        }

        [Route("LotteryBetTypeName")]
        [HttpPost]
        public IActionResult LotteryBetTypeName([FromBody]byte[] parameterSets, [FromBody]byte[] languages)
        {
            var fastMarkets225ResultSet = GetResultSet(
                parameterSets,
                languages,
                (parameter, language) => string.Empty, // TODO: Update latest RemotingRef
                "LotteryBetTypeName");

            return Ok(_serializer.Serialize(fastMarkets225ResultSet));
        }

        private Dictionary<string, Dictionary<string, T>> GetResultSet<T>(
            byte[] parameterSets,
            byte[] languages,
            Func<Dictionary<string, object>, string, T> getRefData,
            string resultKeyName)
        {
            var resultSet = new Dictionary<string, Dictionary<string, T>>();
            var parsedParameterSets = _serializer.Deserialize<List<Dictionary<string, object>>>(parameterSets);
            var parsedLanguages = languages != null ? _serializer.Deserialize<string[]>(languages) : new[] { LanguageHelper.DefaultLanguage };

            foreach (var parameterSet in parsedParameterSets)
            {
                var result = new Dictionary<string, T>();

                foreach (var language in parsedLanguages)
                {
                    result.Add(language, getRefData(parameterSet, language));
                }

                resultSet.Add(parameterSet[resultKeyName].ToString(), result);
            }

            return resultSet;
        }
    }
}