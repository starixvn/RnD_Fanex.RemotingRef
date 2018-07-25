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
        public Dictionary<ParameterSet, Dictionary<string, string>> BetTypeName([FromBody]RequestSyncFormData data)
        {
            var betTypeNameSet = new Dictionary<ParameterSet, Dictionary<string, string>>();

            foreach (var parameterSet in data.ParameterSets)
            {
                var betTypeNames = GetBetTypeNames(data.Languages, parameterSet);

                if (IsValidRefResultData(betTypeNames))
                {
                    betTypeNameSet.Add(parameterSet, betTypeNames);
                }
            }

            return betTypeNameSet;
        }

        private Dictionary<string, string> GetBetTypeNames(string[] languages, ParameterSet parameterSet)
        {
            var betTypeNames = new Dictionary<string, string>();
            var betTypeId = parameterSet.ContainsKey("betTypeId") ? (parameterSet["betTypeId"] as string) : null;
            var resourceId = parameterSet.ContainsKey("resourceId") ? (parameterSet["resourceId"] as string) : null;
            var refNo = parameterSet.ContainsKey("refNo") ? (long?)parameterSet["refNo"] : null;

            foreach (var language in languages)
            {
                betTypeNames.Add(
                    language,
                    _refData.GetBetTypeName(
                        betTypeId,
                        resourceId,
                        refNo,
                        LanguageHelpers.ToRemotingRefLanguageCode(language)));
            }

            return betTypeNames;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> SportName([FromBody]RequestSyncFormData data)
        {
            var sportNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetSportName(parameter["sportTypeId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return sportNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> TeamName([FromBody]RequestSyncFormData data)
        {
            var teamNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetTeamName(parameter["teamId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return teamNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> TeamName3rd([FromBody]RequestSyncFormData data)
        {
            var teamName3rdSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetTeamName3rd(parameter["team3rdId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return teamName3rdSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> LeagueName([FromBody]RequestSyncFormData data)
        {
            var leagueNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetLeagueName(parameter["leagueId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return leagueNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> LeagueName3rd([FromBody]RequestSyncFormData data)
        {
            var leagueName3rdSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetLeagueName3rd(parameter["league3rdId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return leagueName3rdSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> CountryName([FromBody]RequestSyncFormData data)
        {
            var countryNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetCountryName(parameter["countryCode"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return countryNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> OddsTypeName([FromBody]RequestSyncFormData data)
        {
            var oddsTypeNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetOddsTypeName(parameter["oddsTypeId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return oddsTypeNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> ResourceData([FromBody]RequestSyncFormData data)
        {
            var resourceDataSet = GetRefResultSet(
                    data,
                    (parameter, language)
                        => _refData.GetResourceDataByRefIdAndResourceId(
                                parameter["refId"].ToString(),
                                parameter["resourceId"].ToString(),
                                LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return resourceDataSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> ParentIdByBetTypeId([FromBody]RequestSyncFormData data)
        {
            var parentIdSet = GetRefResultSet(
                    data,
                    (parameter, _) => _refData.GetParentIdByBetTypeId(parameter["betTypeId"].ToString()));

            return parentIdSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string[]>> CombinationTypeComboParlay([FromBody]RequestSyncFormData data)
        {
            var combinationTypeSet = GetRefResultSet(
                    data,
                    (parameter, language)
                        => _refData.GetCombinationTypeCombo(
                                parameter["betTeam"].ToString(),
                                parameter["betCheck"].ToString(),
                                (int)parameter["handicap1"],
                                (int)parameter["handicap2"],
                                LanguageHelpers.ToRemotingRefLanguageCode(language)),
                    (result) => result.Any(item => item.Value != null && item.Value.Any(type => !string.IsNullOrEmpty(type))));

            return combinationTypeSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> HorseTeamName([FromBody]RequestSyncFormData data)
        {
            var horseTeamNameSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetHorseTeamName(parameter["codeId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return horseTeamNameSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> CasinoBetItem([FromBody]RequestSyncFormData data)
        {
            var casinoBetItemSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetCasinoBetItem(parameter["itemId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return casinoBetItemSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> RouletteBetItem([FromBody]RequestSyncFormData data)
        {
            var rouletteBetItemSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetRouletteBetItem(parameter["itemId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return rouletteBetItemSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> KenoBetType([FromBody]RequestSyncFormData data)
        {
            var kenoBetTypeSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetKenoBetType(parameter["kenoBetTypeId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return kenoBetTypeSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, Tuple<string, string>>> KenoBetChoice([FromBody] RequestSyncFormData data)
        {
            Tuple<string, string> GetKenoBetChoice(ParameterSet parameter, string language)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(
                        parameter["kenoBetChoiceId"].ToString(),
                        LanguageHelpers.ToRemotingRefLanguageCode(language),
                        out string kenoBetTypeId);

                return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
            }

            var kenoBetChoiceSet = GetRefResultSet(
                 data,
                 getRefData: GetKenoBetChoice,
                 isValidRefResultData: (result) => result.Any(item => item.Value != null && (!string.IsNullOrEmpty(item.Value.Item1) || !string.IsNullOrEmpty(item.Value.Item2))));

            return kenoBetChoiceSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> FastMarketsChoice([FromBody]RequestSyncFormData data)
        {
            var fastMarketChoiceSet = GetRefResultSet(
                    data,
                    (parameter, language)
                        => _refData.GetFastMarketsChoice(
                                parameter["betTypeId"].ToString(),
                                parameter["betTeam"].ToString(),
                                (int)parameter["handicap1"],
                                (int)parameter["handicap2"],
                                LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return fastMarketChoiceSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> FastMarkets225Result([FromBody]RequestSyncFormData data)
        {
            var fastMarkets225ResultSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetFastMarkets225Result(parameter["resultId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return fastMarkets225ResultSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, Tuple<string, string>>> LotteryBetChoice([FromBody]RequestSyncFormData data)
        {
            Tuple<string, string> GetLotteryBetChoice(ParameterSet parameter, string language)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(
                        parameter["betChoiceId"].ToString(),
                        LanguageHelpers.ToRemotingRefLanguageCode(language),
                        out string lotteryBetType);

                return new Tuple<string, string>(lotteryBetChoice, lotteryBetType);
            }

            var lotteryBetChoiceSet = GetRefResultSet(
                  data,
                  getRefData: GetLotteryBetChoice,
                  isValidRefResultData: (result) => result.Any(item => !string.IsNullOrEmpty(item.Value.Item1)));

            return lotteryBetChoiceSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> LotteryBetType([FromBody]RequestSyncFormData data)
        {
            var lotteryBetTypeSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetLotteryBetType(parameter["betTypeId"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return lotteryBetTypeSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> BetRadarMatchCodeFormat([FromBody]RequestSyncFormData data)
        {
            var refResultSet = GetRefResultSet(
                    data,
                    (parameter, language) => _refData.GetBetRadarMatchCodeFormat(parameter["matchCode"].ToString(), LanguageHelpers.ToRemotingRefLanguageCode(language)));

            return refResultSet;
        }

        [HttpPost]
        public Dictionary<ParameterSet, Dictionary<string, string>> DisplayedTypeId([FromBody]RequestSyncFormData data)
        {
            var refResultSet = GetRefResultSet(
                    data,
                    (parameter, _) => _refData.GetDisplayedTypeId(parameter["betTypeId"].ToString()));

            return refResultSet;
        }

        private static Dictionary<ParameterSet, Dictionary<string, T>> GetRefResultSet<T>(
            RequestSyncFormData data,
            Func<ParameterSet, string, T> getRefData,
            Func<Dictionary<string, T>, bool> isValidRefResultData = null)
        {
            var resultSet = new Dictionary<ParameterSet, Dictionary<string, T>>();
            data.Languages = data.Languages ?? new[] { "en-US" };
            var checkValidRefResultData = isValidRefResultData ?? IsValidRefResultData;

            foreach (var parameterSet in data.ParameterSets)
            {
                var result = new Dictionary<string, T>();

                foreach (var language in data.Languages)
                {
                    result.Add(language, getRefData(parameterSet, language));
                }

                if (checkValidRefResultData(result))
                {
                    resultSet.Add(parameterSet, result);
                }
            }

            return resultSet;
        }

        private static bool IsValidRefResultData<T>(IDictionary<string, T> result)
            => result.Any(item => !string.IsNullOrEmpty(item.Value as string));
    }
}