namespace Fanex.RemotingRef.Service.Controllers
{
    using System.Collections.Generic;
    using global::RemotingRef;
    using MessagePack;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class RefController : Controller
    {
        private readonly IRefData _refData;

        public RefController(IRefData refData)
        {
            _refData = refData;
        }

        public object BetTypeName(string betTypeId, string[] langs, string resourceId = null, long refNo = 0, string defaultValue = "")
        {
            var betTypeName = new Dictionary<string, string>();

            foreach (var lang in langs)
            {
                betTypeName.Add(lang, _refData.GetBetTypeName(betTypeId, GetCurrentTWLanguage(lang), defaultValue, resourceId, refNo));
            }

            return MessagePackSerializer.Serialize(betTypeName);
        }

        public object SportName(string betTypeId, string[] langs, string defaultValue = "")
        {
            var sportName = new Dictionary<string, string>();

            foreach (var lang in langs)
            {
                sportName.Add(lang, _refData.GetSportName(betTypeId, GetCurrentTWLanguage(lang), defaultValue));
            }

            return MessagePackSerializer.Serialize(sportName);
        }

        public object TeamName(string betTypeId, string[] langs, string defaultValue = "")
        {
            var teamName = new Dictionary<string, string>();

            foreach (var lang in langs)
            {
                teamName.Add(lang, _refData.GetTeamName(betTypeId, GetCurrentTWLanguage(lang), defaultValue));
            }

            return MessagePackSerializer.Serialize(teamName);
        }

        public object LeagueName(string betTypeId, string[] langs, string defaultValue = "")
        {
            var leagueName = new Dictionary<string, string>();

            foreach (var lang in langs)
            {
                leagueName.Add(lang, _refData.GetLeagueName(betTypeId, GetCurrentTWLanguage(lang), defaultValue));
            }

            return MessagePackSerializer.Serialize(leagueName);
        }

        private static string GetCurrentTWLanguage(string lang)
        {
            switch (lang)
            {
                case "en-US": return "en";
                case "zh-TW": return "ch"; // Traditional
                case "zh-CN": return "cs"; // Simplified
                case "ja-JP": return "jp";
                case "th-TH": return "th"; // Thai
                case "ko-KR": return "ko"; // Korean
                case "vi-VN": return "vn"; // Vietnamese
                case "zh-Hans": return "zhcn"; // New Simplified Chinese
                default: return "en";
            }
        }
    }
}