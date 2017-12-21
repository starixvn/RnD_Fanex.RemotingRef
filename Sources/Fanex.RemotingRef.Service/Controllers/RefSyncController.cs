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
        public IActionResult BetTypeName([FromBody] Dictionary<string, object>[] parameters, string[] languages)
        {
            var betTypeNamesList = new List<Dictionary<string, string>>();

            foreach (var parameter in parameters)
            {
                var betTypeNames = GetBetTypeNames(languages, parameter);

                betTypeNamesList.Add(betTypeNames);
            }

            return new OkObjectResult(_serializer.Serialize(betTypeNamesList));
        }

        private Dictionary<string, string> GetBetTypeNames(string[] languages, Dictionary<string, object> parameter)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                if (parameter.Keys.Count == 1)
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(parameter["betTypeId"].ToString(), LanguageHelper.GenerateTaiwanLanguage(language), string.Empty));
                }
                else if (parameter.Keys.Count == 2 && parameter.ContainsKey("refNo"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameter["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        (long)parameter["refNo"],
                        string.Empty));
                }
                else if (parameter.Keys.Count == 2 && parameter.ContainsKey("resourceId"))
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                        parameter["betTypeId"].ToString(),
                        LanguageHelper.GenerateTaiwanLanguage(language),
                        string.Empty,
                        parameter["resourceId"].ToString()));
                }
                else
                {
                    betTypeNames.Add(language, _refData.GetBetTypeName(
                       parameter["betTypeId"].ToString(),
                       LanguageHelper.GenerateTaiwanLanguage(language),
                       string.Empty,
                       parameter["resourceId"].ToString(),
                        (long)parameter["refNo"]));
                }
            }

            return betTypeNames;
        }
    }
}