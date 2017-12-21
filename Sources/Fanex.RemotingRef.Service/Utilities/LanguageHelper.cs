namespace Fanex.RemotingRef.Service.Utilities
{
    public static class LanguageHelper
    {
        public static string GenerateTaiwanLanguage(string lang)
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