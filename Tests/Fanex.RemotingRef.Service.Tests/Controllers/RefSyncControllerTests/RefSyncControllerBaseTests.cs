namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using NSubstitute;

    public class RefSyncControllerBaseTests : IDisposable
    {
        protected const string DefaultLanguageCode = LanguageHelper.DefaultLanguage;

        protected static readonly string[] StandardLanguageCodes = { DefaultLanguageCode, "zh-TW" };

        protected static readonly Dictionary<string, string> MappedRemotingRefLanguageCodes = new Dictionary<string, string>
        {
            ["en-US"] = "en",
            ["zh-TW"] = "ch"
        };

        private bool _disposed;

        protected RefSyncControllerBaseTests()
        {
            RefData = Substitute.For<IRefDataHelper>();
            RefSyncController = new RefSyncController(RefData);
            Fixture = new Fixture();
        }

        protected IRefDataHelper RefData { get; }

        protected RefSyncController RefSyncController { get; }

        protected Fixture Fixture { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RefSyncController.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
            => Dispose(true);
    }
}