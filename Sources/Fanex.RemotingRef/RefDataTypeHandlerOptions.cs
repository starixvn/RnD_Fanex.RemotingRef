namespace Fanex.RemotingRef
{
    using Delegates;
    using TranslationStorage;

    public class RefDataTypeHandlerOptions : LocalStorageOptions
    {
        public const string DefaultLanguage = "en-US";
        private static string[] supportedLanguageCodes = { DefaultLanguage, "zh-TW", "zh-CN", "ja-JP", "th-TH", "ko-KR", "vi-VN", "zh-Hans" };

        public int SyncBatchSize { get; set; }
            = 100;

        internal static string[] SupportedLanguageCodes
        {
            get => IsMultiLanguagesSupported ? supportedLanguageCodes : new[] { DefaultLanguage };
            set => supportedLanguageCodes = value;
        }

        internal static bool IsMultiLanguagesSupported { get; set; }
            = true;

        internal RequestTranslationSetDelegate ValueFactory { get; set; }

        internal SyncTranslationSetsDelegate SyncHandler { get; set; }
    }
}