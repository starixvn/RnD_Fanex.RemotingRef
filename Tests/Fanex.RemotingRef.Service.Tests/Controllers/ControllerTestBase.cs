namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Fanex.RemotingRef.NetCore.Utilities.RefData;
    using NSubstitute;

    public abstract class ControllerTestBase : IDisposable
    {
        public const string DefaultLanguageCode = LanguageHelpers.DefaultLanguage;

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
            "zh-CHS",
            "zh-TW"
        };

        public IRefDataHelper RefData { get; }

        public ITeam3rdService Team3rdService { get; } = Substitute.For<ITeam3rdService>();

        public ILeague3rdService League3rdService { get; } = Substitute.For<ILeague3rdService>();

        public Fixture Fixture { get; }

        public static bool ValidateTextSet(TranslationSet textSet)
            => textSet.Any(pair => !string.IsNullOrEmpty(pair.Value as string));

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
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}