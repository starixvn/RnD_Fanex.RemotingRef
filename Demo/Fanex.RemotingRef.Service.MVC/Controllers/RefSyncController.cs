namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Models;
    using global::RemotingRef;

    public class RefSyncController : ApiController
    {
        private readonly IRefData _refData;

        public RefSyncController()
        {
            _refData = new RefNewData.RefNewDataHandler();
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> BetTypeName([FromBody]RequestSyncFormData data)
        {
            var betTypeNameSet = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();

            foreach (var parameterSet in data.ParameterSets)
            {
                var betTypeNames = GetBetTypeNames(data.Languages, parameterSet);

                betTypeNameSet.Add(parameterSet, betTypeNames);
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
                    betTypeNames.Add(language, _refData.GetBetTypeName(parameterSet["betTypeId"].ToString(), GenerateTaiwanLanguage(language), string.Empty));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("refNo"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        GenerateTaiwanLanguage(language),
                        (long)parameterSet["refNo"],
                        string.Empty));
                }
                else if (parameterSet.Keys.Count == 2 && parameterSet.ContainsKey("resourceId"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameterSet["betTypeId"].ToString(),
                        GenerateTaiwanLanguage(language),
                        string.Empty,
                        parameterSet["resourceId"].ToString()));
                }
                else
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                       parameterSet["betTypeId"].ToString(),
                       GenerateTaiwanLanguage(language),
                       string.Empty,
                       parameterSet["resourceId"].ToString(),
                        (long)parameterSet["refNo"]));
                }
            }

            return betTypeNames;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> SportName([FromBody]RequestSyncFormData data)
        {
            var sportNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetSportName(parameter["sportTypeId"].ToString(), GenerateTaiwanLanguage(language), string.Empty));

            return sportNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> TeamName([FromBody]RequestSyncFormData data)
        {
            var teamNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetTeamName(parameter["teamId"].ToString(), GenerateTaiwanLanguage(language), string.Empty));

            return teamNameSet;
        }

        [Route("LeagueName")]
        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> LeagueName([FromBody]RequestSyncFormData data)
        {
            var leagueNameSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetLeagueName(parameter["leagueId"].ToString(), GenerateTaiwanLanguage(language), string.Empty));

            return leagueNameSet;
        }

        [HttpPost]
        public Dictionary<Dictionary<string, object>, Dictionary<string, string>> ResourceData([FromBody]RequestSyncFormData data)
        {
            var resourceDataSet = GetResultSet(
                data,
                (parameter, language)
                    => _refData.GetResourceDataByRefIdAndResourceId(parameter["refId"].ToString(), parameter["resourceId"].ToString(), GenerateTaiwanLanguage(language), string.Empty));

            return resourceDataSet;
        }

        private static Dictionary<Dictionary<string, object>, Dictionary<string, T>> GetResultSet<T>(
            RequestSyncFormData data,
            Func<Dictionary<string, object>, string, T> getRefData)
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

                resultSet.Add(parameterSet, result);
            }

            return resultSet;
        }

        private static string GenerateTaiwanLanguage(string lang)
        {
            switch (lang)
            {
                case "en-US": return "en"; // English
                case "zh-TW": return "ch"; // Traditional
                case "zh-CN": return "cs"; // Simplified
                case "ja-JP": return "jp"; // Japanese
                case "th-TH": return "th"; // Thai
                case "ko-KR": return "ko"; // Korean
                case "vi-VN": return "vn"; // Vietnamese
                case "zh-Hans": return "zhcn"; // New Simplified Chinese
                default: return "en";
            }
        }
    }
}