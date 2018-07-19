#pragma warning disable S1118 // Utility classes should not have public constructors

namespace Fanex.RemotingRef.Integration.Tests
{
    using System;
    using System.Configuration;

    public class RemotingRefServiceFixture
    {
        private static readonly string RemotingRefServiceUrl = ConfigurationManager.AppSettings["RemotingRefServiceTest"];

        public RemotingRefServiceFixture()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri(RemotingRefServiceUrl))
                .FromApplication("UnitTest_AppName")
                .SupportOnlyDefaultLanguage();
        }
    }
}