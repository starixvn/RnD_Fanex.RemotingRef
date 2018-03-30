namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;

    public class RefSyncController : ApiController
    {
        private readonly IRefDataHelper _refData = new RefDataHelper();

        public RefSyncController()
            : this(null)
        {
        }

        public RefSyncController(IRefDataHelper refData)
        {
            _refData = refData ?? _refData;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> BetTypeName([FromBody]RequestSyncFormData data)
        {
            var betTypeNameSet = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();

            foreach (var parameterSet in data.ParameterSets)
            {
                var betTypeNames = GetBetTypeNames(data.Languages, parameterSet);

                if (IsValidRefResultData<string>()(betTypeNames))
                {
                    betTypeNameSet.Add(parameterSet, betTypeNames);
                }
            }

            return betTypeNameSet;
        }

        private Dictionary<string, string> GetBetTypeNames(string[] languages, Dictionary<string, object> parameterSet)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                if (parameterSet.Keys.Count == 1)
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(parameterSet["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("refNo"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        (long)parameterSet["refNo"]));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("resourceId"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        parameterSet["resourceId"].ToString()));
                }
                else
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                       parameterSet["betTypeId"].ToString(),
                       LanguageHelper.GenerateTaiwanLanguage(language),
                       parameterSet["resourceId"].ToString(),
                        (long)parameterSet["refNo"]));
                }
            }

            return betTypeNames;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> SportName([FromBody]RequestSyncFormData data)
        {
            var sportNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetSportName(parameter["sportTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return sportNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> TeamName([FromBody]RequestSyncFormData data)
        {
            var teamNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetTeamName(parameter["teamId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return teamNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> TeamName3rd([FromBody]RequestSyncFormData data)
        {
            var teamName3rdSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetTeamName(parameter["team3rdId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return teamName3rdSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> LeagueName([FromBody]RequestSyncFormData data)
        {
            var leagueNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetLeagueName(parameter["leagueId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return leagueNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> LeagueName3rd([FromBody]RequestSyncFormData data)
        {
            var leagueName3rdSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetLeagueName(parameter["league3rdId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return leagueName3rdSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> ResourceData([FromBody]RequestSyncFormData data)
        {
            var resourceDataSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetResourceDataByRefIdAndResourceId(
                        parameter["refId"].ToString(),
                        parameter["resourceId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language)));

            return resourceDataSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> CountryName([FromBody]RequestSyncFormData data)
        {
            var countryNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetCountryName(parameter["countryCode"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return countryNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> OddsTypeName([FromBody]RequestSyncFormData data)
        {
            var oddsTypeNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetOddsTypeName(parameter["oddsTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return oddsTypeNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> ParentIdByBetTypeId([FromBody]RequestSyncFormData data)
        {
            var parentIdSet = GetRefResultSet(
                data,
                (parameter, _) => _refData.GetParentIdByBetTypeId(parameter["betTypeId"].ToString()));

            return parentIdSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string[]>> CombinationTypeComboParlay([FromBody]RequestSyncFormData data)
        {
            var combinationTypeSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetCombinationTypeCombo(
                        parameter["betTeam"].ToString(),
                        parameter["betCheck"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)),
                (result) => result.Any(item => item.Value.Any(type => !string.IsNullOrEmpty(type))));

            return combinationTypeSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> HorseTeamName([FromBody]RequestSyncFormData data)
        {
            var horseTeamNameSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetHorseTeamName(parameter["codeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return horseTeamNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> CasinoBetItem([FromBody]RequestSyncFormData data)
        {
            var casinoBetItemSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetCasinoBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return casinoBetItemSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> RouletteBetItem([FromBody]RequestSyncFormData data)
        {
            var rouletteBetItemSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetRouletteBetItem(parameter["itemId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return rouletteBetItemSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> KenoBetType([FromBody]RequestSyncFormData data)
        {
            var kenoBetTypeSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetKenoBetType(parameter["kenoBetTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return kenoBetTypeSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>> KenoBetChoice([FromBody] RequestSyncFormData data)
        {
            var kenoBetChoiceSet = GetRefResultSet(
                data,
                getRefData: (parameter, language) =>
                {
                    var kenoBetChoice = _refData.GetKenoBetChoice(
                        parameter["kenoBetChoiceId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        out string kenoBetTypeId);

                    return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
                },
                isValidRefResultData: (result) => result.Any(item => !string.IsNullOrEmpty(item.Value.Item1)));

            return kenoBetChoiceSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> FastMarketsChoice([FromBody]RequestSyncFormData data)
        {
            var fastMarketChoiceSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetFastMarketsChoice(
                        parameter["betTypeId"].ToString(),
                        parameter["betTeam"].ToString(),
                        (int)parameter["handicap1"],
                        (int)parameter["handicap2"],
                        LanguageHelper.GenerateTaiwanLanguage(language)));

            return fastMarketChoiceSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> FastMarkets225Result([FromBody]RequestSyncFormData data)
        {
            var fastMarkets225ResultSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetFastMarkets225Result(parameter["resultId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return fastMarkets225ResultSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>> LotteryBetChoice([FromBody]RequestSyncFormData data)
        {
            var lotteryBetChoiceSet = GetRefResultSet(
                  data,
                  getRefData: (parameter, language) =>
                  {
                      var lotteryBetChoice = _refData.GetLotteryBetChoice(
                          parameter["betChoiceId"].ToString(),
                          LanguageHelper.GenerateTaiwanLanguage(language),
                          out string lotteryBetType);

                      return new Tuple<string, string>(lotteryBetChoice, lotteryBetType);
                  },
                  isValidRefResultData: (result) => result.Any(item => !string.IsNullOrEmpty(item.Value.Item1)));

            return lotteryBetChoiceSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> LotteryBetTypeName([FromBody]RequestSyncFormData data)
        {
            var lotteryBetTypeSet = GetRefResultSet(
                data,
                (parameter, language)
                    => _refData.GetLotteryBetType(parameter["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language)));

            return lotteryBetTypeSet;
        }

        private static Dictionary<Dictionary<string, object>, Dictionary<string, T>> GetRefResultSet<T>(
            RequestSyncFormData data,
            Func<Dictionary<string, object>, string, T> getRefData,
            Func<Dictionary<string, T>, bool> isValidRefResultData = null)
        {
            var resultSet = new Dictionary<Dictionary<string, object>, Dictionary<string, T>>();
            data.Languages = data.Languages ?? new[] { "en-US" };

            foreach (var parameterSet in data.ParameterSets)
            {
                var result = new Dictionary<string, T>();

                foreach (var language in data.Languages)
                {
                    result.Add(language, getRefData(parameterSet, language));
                }

                if (isValidRefResultData == null ? IsValidRefResultData<T>()(result) : isValidRefResultData(result))
                {
                    resultSet.Add(parameterSet, result);
                }
            }

            return resultSet;
        }

        private static Func<IDictionary<string, T>, bool> IsValidRefResultData<T>()
          => (result) => result.Any(item => !string.IsNullOrEmpty(item.Value.ToString()));
    }
}