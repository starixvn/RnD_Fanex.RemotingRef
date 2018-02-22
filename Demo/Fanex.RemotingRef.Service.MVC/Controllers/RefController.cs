namespace Fanex.RemotingRef.Service.MVC.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using global::RemotingRef;

    public class RefController : ApiController
    {
        private readonly IRefData _refData;

        public RefController()
        {
            _refData = new RefNewData.RefNewDataHandler();
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeName(string betTypeId, [FromUri] string[] languages)
        {
            var betTypeNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                betTypeNames.Add(language, _refData.GetBetTypeName(betTypeId, GenerateTaiwanLanguage(language), string.Empty));
            }

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> SportName(string sportTypeId, [FromUri] string[] languages)
        {
            var sportNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                sportNames.Add(language, _refData.GetSportName(sportTypeId, GenerateTaiwanLanguage(language), string.Empty));
            }

            return sportNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName(string teamId, [FromUri] string[] languages)
        {
            var teamNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                teamNames.Add(language, _refData.GetTeamName(teamId, GenerateTaiwanLanguage(language), string.Empty));
            }

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName(string leagueId, [FromUri] string[] languages)
        {
            var leagueNames = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                leagueNames.Add(language, _refData.GetLeagueName(leagueId, GenerateTaiwanLanguage(language), string.Empty));
            }

            return leagueNames;
        }

        [HttpGet]
        public Dictionary<string, string> ResourceData(string refId, string resourceId, [FromUri] string[] languages)
        {
            var resourceData = new Dictionary<string, string>();

            foreach (var language in languages)
            {
                resourceData.Add(language, _refData.GetResourceDataByRefIdAndResourceId(refId, resourceId, GenerateTaiwanLanguage(language), string.Empty));
            }

            return resourceData;
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