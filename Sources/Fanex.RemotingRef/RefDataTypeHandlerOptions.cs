namespace Fanex.RemotingRef
{
    using Fanex.RemotingRef.Delegates;
    using Fanex.RemotingRef.TranslationStorage;

    public class RefDataTypeHandlerOptions : LocalStorageOptions
    {
        public const string DefaultLanguage = "en-US";

        internal static readonly string[] DefaultSupportedLanguageCodes =
        {
            DefaultLanguage,
            "zh-TW",
            "zh-CN",
            "ja-JP",
            "th-TH",
            "ko-KR",
            "vi-VN",
            "zh-Hans"
        };

        private static string[] supportedLanguageCodes = DefaultSupportedLanguageCodes;

        public int SyncBatchSize { get; set; }
            = 100;

        public int NumberOfRetries { get; set; }
            = 5;

        internal static string ApplicationName { get; set; }

        internal static string[] SupportedLanguageCodes
        {
            get => IsMultiLanguagesSupported ? supportedLanguageCodes : new[] { DefaultLanguage };
            set => supportedLanguageCodes = value;
        }

        internal static bool IsMultiLanguagesSupported { get; set; }
            = true;

        internal RequestTranslationSetDelegate RequestHandler { get; set; }

        internal SyncTranslationSetsDelegate SyncHandler { get; set; }
    }
}