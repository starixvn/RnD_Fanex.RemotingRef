namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using NSubstitute;

    public abstract class ControllerTestBase : IDisposable
    {
        public const string DefaultLanguageCode = LanguageHelper.DefaultLanguage;

        private bool _disposed;

        protected ControllerTestBase()
        {
            RefData = Substitute.For<IRefDataHelper>();
            Fixture = new Fixture();
        }

        public static string[] StandardLanguageCodes => new[]
        {
            DefaultLanguageCode,
            "ja-JP",
            "ko-KR",
            "th-TH",
            "vi-VN",
            "zh-CN",
            "zh-Hans",
            "zh-TW"
        };

        public static Dictionary<string, string> MappedRemotingRefLanguageCodes => new Dictionary<string, string>
        {
            [DefaultLanguageCode] = "en",
            ["ja-JP"] = "jp",
            ["ko-KR"] = "ko",
            ["th-TH"] = "th",
            ["vi-VN"] = "vn",
            ["zh-CN"] = "cs",
            ["zh-Hans"] = "zhcn",
            ["zh-TW"] = "ch"
        };

        public IRefDataHelper RefData { get; }

        public Fixture Fixture { get; }

        protected abstract void DisposeController();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeController();
                }

                _disposed = true;
            }
        }

        public void Dispose()
            => Dispose(true);
    }
}