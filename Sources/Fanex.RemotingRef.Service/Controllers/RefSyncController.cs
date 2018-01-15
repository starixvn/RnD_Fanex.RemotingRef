namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RefSyncController : Controller
    {
        private readonly IRefDataHelper _refData;

        public RefSyncController(IRefDataHelper refData)
        {
            _refData = refData;
        }

        [Route("BetTypeName")]
        [HttpPost]
        public IActionResult BetTypeName([FromBody]RequestSyncFormData data)
        {
            var betTypeNameSet = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();

            foreach (var parameterSet in data.ParameterSets)
            {
                var betTypeNames = GetBetTypeNames(data.Languages, parameterSet);

                betTypeNameSet.Add(parameterSet, betTypeNames);
            }

            return Ok(betTypeNameSet);
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
        public IActionResult SportName([FromBody]RequestSyncFormData data)
        {
            var sportNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetSportName(parameter["sportTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(sportNameSet);
        }

        [Route("TeamName")]
        [HttpPost]
        public IActionResult TeamName([FromBody]RequestSyncFormData data)
        {
            var teamNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetTeamName(parameter["teamId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(teamNameSet);
        }

        [Route("LeagueName")]
        [HttpPost]
        public IActionResult LeagueName([FromBody]RequestSyncFormData data)
        {
            var leagueNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetLeagueName(parameter["leagueId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(leagueNameSet);
        }

        [Route("CountryName")]
        [HttpPost]
        public IActionResult CountryName([FromBody]RequestSyncFormData data)
        {
            var countryNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetCountryName(parameter["countryCode"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(countryNameSet);
        }

        [Route("OddsTypeName")]
        [HttpPost]
        public IActionResult OddsTypeName([FromBody]RequestSyncFormData data)
        {
            var oddsTypeNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetOddsTypeName(parameter["oddsTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(oddsTypeNameSet);
        }

        [Route("ResourceData")]
        [HttpPost]
        public IActionResult ResourceData([FromBody]RequestSyncFormData data)
        {
            var resourceDataSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetResourceDataByRefIdAndResourceId(parameter["refId"].ToString(), parameter["resourceId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(resourceDataSet);
        }

        [Route("ParentIdByBetTypeId")]
        [HttpPost]
        public IActionResult ParentIdByBetTypeId([FromBody]RequestSyncFormData data)
        {
            var parentIdSet = GetResultSet(
                data,
                (parameter, _) => _refData.GetParentIdByBetTypeId(parameter["betTypeId"].ToString(), string.Empty));

            return Ok(parentIdSet);
        }

        [Route("CombinationTypeComboParlay")]
        [HttpPost]
        public IActionResult CombinationTypeComboParlay([FromBody]RequestSyncFormData data)
        {
            var combinationTypeSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetCombinationTypeCombo(
                        parameter["betTeam"].ToString(),
                        parameter["betCheck"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)));

            return Ok(combinationTypeSet);
        }

        [Route("HorseTeamName")]
        [HttpPost]
        public IActionResult HorseTeamName([FromBody]RequestSyncFormData data)
        {
            var horseTeamNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetHorseTeamName(parameter["codeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(horseTeamNameSet);
        }

        [Route("CasinoBetItem")]
        [HttpPost]
        public IActionResult CasinoBetItem([FromBody]RequestSyncFormData data)
        {
            var casinoBetItemSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetCasinoBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(casinoBetItemSet);
        }

        [Route("RouletteBetItem")]
        [HttpPost]
        public IActionResult RouletteBetItem([FromBody]RequestSyncFormData data)
        {
            var rouletteBetItemSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetRouletteBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(rouletteBetItemSet);
        }

        [Route("KenoBetType")]
        [HttpPost]
        public IActionResult KenoBetType([FromBody]RequestSyncFormData data)
        {
            var kenoBetTypeSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetKenoBetType(parameter["kenoBetTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(kenoBetTypeSet);
        }

        [Route("KenoBetChoice")]
        [HttpPost]
        public IActionResult KenoBetChoice([FromBody]RequestSyncFormData data)
        {
            var kenoBetChoiceSet = GetResultSet(
                data,
                (parameter, language) =>
                    {
                        var kenoBetChoice = _refData.GetKenoBetChoice(
                            parameter["kenoBetChoiceId"].ToString(),
                            LanguageHelper.GenerateTaiwanLanguage(language),
                            string.Empty,
                            out string kenoBetTypeId);

                        return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
                    });

            return Ok(kenoBetChoiceSet);
        }

        [Route("FastMarketsChoice")]
        [HttpPost]
        public IActionResult FastMarketsChoice([FromBody]RequestSyncFormData data)
        {
            var fastMarketChoiceSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetFastMarketsChoice(
                        parameter["betTypeId"].ToString(),
                        parameter["betTeam"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)));

            return Ok(fastMarketChoiceSet);
        }

        [Route("FastMarkets225Result")]
        [HttpPost]
        public IActionResult FastMarkets225Result([FromBody]RequestSyncFormData data)
        {
            var fastMarkets225ResultSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetFastMarkets225Result(parameter["resultId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return Ok(fastMarkets225ResultSet);
        }

        [Route("LotteryBetChoice")]
        [HttpPost]
        public IActionResult LotteryBetChoice([FromBody]RequestSyncFormData data)
        {
            var lotteryBetChoiceSet = GetResultSet(
                  data,
                  (parameter, language) =>
                  {
                      var lotteryBetChoice = _refData.GetLotteryBetChoice(
                          parameter["betChoiceId"].ToString(),
                          LanguageHelper.GenerateTaiwanLanguage(language),
                          string.Empty,
                          out string lotteryBetType);

                      return new Tuple<string, string>(lotteryBetChoice, lotteryBetType);
                  });

            return Ok(lotteryBetChoiceSet);
        }

        [Route("LotteryBetTypeName")]
        [HttpPost]
        public IActionResult LotteryBetTypeName([FromBody]RequestSyncFormData data)
        {
            var lotteryBetTypeSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetLotteryBetType(parameter["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));

            return Ok(lotteryBetTypeSet);
        }

        private static Dictionary<Dictionary<string, object>, Dictionary<string, T>> GetResultSet<T>(
            RequestSyncFormData data,
            Func<Dictionary<string, object>, string, T> getRefData)
        {
            var resultSet = new Dictionary<Dictionary<string, object>, Dictionary<string, T>>();
            data.Languages = data.Languages ?? new[] { LanguageHelper.DefaultLanguage };

            foreach (var parameterSet in data.ParameterSets)
            {
                var result = new Dictionary<string, T>();

                foreach (var language in data.Languages)
                {
                    result.Add(language, getRefData(parameterSet, language));
                }

                resultSet.Add(parameterSet, result);
            }

            return resultSet;
        }
    }
}