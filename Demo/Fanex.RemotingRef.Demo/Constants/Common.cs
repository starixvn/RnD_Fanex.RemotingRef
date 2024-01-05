namespace Fanex.RemotingRef.Demo.Constants
{
    public static class Common
    {
        public const string DefaultResourceId = "0";
        public const string DefaultLanguage = "en-US";

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
                case "zh-CHS": return "zhcn"; // New Simplified Chinese
                default: return "en";
            }
        }
    }
}