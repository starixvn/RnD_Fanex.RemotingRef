namespace Fanex.RemotingRef.Service.Utilities
{
    public static class LanguageHelpers
    {
        public const string DefaultLanguage = "en-US";

        public static string[] ToRefLanguageCodes(string[] standardLanguageCodes)
        {
            var remotingRefLanguageCodes = new string[standardLanguageCodes.Length];

            for (var counter = 0; counter < standardLanguageCodes.Length; counter++)
            {
                remotingRefLanguageCodes[counter] = ToRemotingRefLanguageCode(standardLanguageCodes[counter]);
            }

            return remotingRefLanguageCodes;
        }

        public static string ToRemotingRefLanguageCode(string standardLanguageCode)
        {
            switch (standardLanguageCode)
            {
                case DefaultLanguage: return "en"; // English
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