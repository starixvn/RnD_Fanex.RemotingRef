namespace Fanex.RemotingRef
{
    using System;
    using Fanex.RemotingRef.Delegates;
    using Fanex.RemotingRef.TranslationStorage;

    public class RefDataTypeHandlerOptions : LocalStorageOptions
    {
        private const string NullApplicationNameErrorMessage = "Please call RefDataTypeContainer.FromApplication(\"YourSite_Name\") to set your application name, this information is required.";
        public const string DefaultLanguage = "en-US";
        private static string applicationName;

        private static string[] supportedLanguageCodes = { DefaultLanguage, "zh-TW", "zh-CN", "ja-JP", "th-TH", "ko-KR", "vi-VN", "zh-Hans" };

        public int SyncBatchSize { get; set; } = 100;

        internal static string ApplicationName
        {
            get => applicationName ?? throw new InvalidOperationException(NullApplicationNameErrorMessage);
            set
            {
                applicationName = value;
            }
        }

        internal static string[] SupportedLanguageCodes
        {
            get => IsMultiLanguagesSupported ? supportedLanguageCodes : new[] { DefaultLanguage };
            set => supportedLanguageCodes = value;
        }

        internal static bool IsMultiLanguagesSupported { get; set; } = true;

        internal RequestTranslationSetDelegate RequestHandler { get; set; }

        internal SyncTranslationSetsDelegate SyncHandler { get; set; }
    }
}