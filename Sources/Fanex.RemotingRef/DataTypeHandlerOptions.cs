namespace Fanex.RemotingRef
{
    using Delegates;
    using TranslationStorage;

    public class DataTypeHandlerOptions : LocalStorageOptions
    {
        public const string DefaultLanguage = "en-US";
        private static string[] supportedLanguageCodes = { DefaultLanguage, "zh-TW", "zh-CN", "ja-JP", "th-TH", "ko-KR", "vi-VN", "zh-Hans" };

        public static string[] SupportedLanguageCodes
        {
            get => IsMultiLanguagesSupported ? supportedLanguageCodes : new[] { DefaultLanguage };
            internal set => supportedLanguageCodes = value;
        }

        public static bool IsMultiLanguagesSupported { get; internal set; }
            = true;

        internal RequestTranslationSetDelegate ValueFactory { get; set; }

        internal SyncDataDelegate SyncDataHandler { get; set; }

        public int UpdatingBatchSize { get; set; }
            = 100;
    }
}