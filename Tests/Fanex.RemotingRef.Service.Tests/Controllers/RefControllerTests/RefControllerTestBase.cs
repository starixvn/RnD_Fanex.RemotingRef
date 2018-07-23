namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using NSubstitute;

    public class RefControllerTestBase : IDisposable
    {
        protected const string DefaultLanguageCode = LanguageHelper.DefaultLanguage;

        protected static readonly string[] StandardLanguageCodes = { DefaultLanguageCode, "zh-TW" };
        protected static readonly Dictionary<string, string> MappedRemotingRefLanguageCodes = new Dictionary<string, string>
        {
            ["en-US"] = "en",
            ["zh-TW"] = "ch"
        };

        private bool _disposed;

        protected RefControllerTestBase()
        {
            RefData = Substitute.For<IRefDataHelper>();
            RefController = new RefController(RefData);
            Fixture = new Fixture();
        }

        protected IRefDataHelper RefData { get; }

        protected RefController RefController { get; }

        protected Fixture Fixture { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RefController.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
            => Dispose(true);
    }
}