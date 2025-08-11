namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using System;
    using System.Linq;

    public abstract class ControllerTestBase : IDisposable
    {
        public const string DefaultLanguageCode = LanguageHelpers.DefaultLanguage;

        private bool _disposed;

        protected ControllerTestBase()
        {
            RefData = Substitute.For<IRefDataWapper>();
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

        public IRefDataWapper RefData { get; }

        public ITeam3rdService Team3rdService { get; } = Substitute.For<ITeam3rdService>();

        public ILeague3rdService League3rdService { get; } = Substitute.For<ILeague3rdService>();

        public IBettypeService BettypeService { get; } = Substitute.For<IBettypeService>();

        public ISportService SportService { get; } = Substitute.For<ISportService>();

        public IRefResourceService RefResourceService { get; } = Substitute.For<IRefResourceService>();

        public IOptions<AppSettings> AppSettings { get; } = Substitute.For<IOptions<AppSettings>>();

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