namespace Fanex.RemotingRef.Service.Controllers
{
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
        public IActionResult BetTypeName([FromBody] Dictionary<string, object>[] parameterSets, string[] languages)
        {
            var betTypeNamesList = new Dictionary<string, Dictionary<string, string>>();

            foreach (var parameterSet in parameterSets)
            {
                var betTypeNames = GetBetTypeNames(languages, parameterSet);

                betTypeNamesList.Add(parameterSet["betTypeId"].ToString(), betTypeNames);
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNamesList));
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
    }
}