namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using TranslationStorage;

    public class DataTypeHandlerOptions : MemoryStorageOptions
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

        public HashKeyDelegate KeyFactory { get; set; }

        public RequestTranslationSetDelegate ValueFactory { get; set; }

        public SyncDataDelegate SyncDataHandler { get; set; }

        public int UpdatingBatchSize { get; set; }
            = 100;
    }
}