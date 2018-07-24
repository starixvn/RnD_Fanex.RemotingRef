namespace Fanex.RemotingRef.Integration.Tests
{
    using System;
    using System.Configuration;

    public class ServiceIntegrationFixture
    {
        public static readonly string RemotingRefServiceUrl = ConfigurationManager.AppSettings["RemotingRefServiceTest"];

#pragma warning disable S1118 // Utility classes should not have public constructors

        public ServiceIntegrationFixture()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri(RemotingRefServiceUrl))
                .FromApplication("UnitTest_AppName")
                .SupportOnlyDefaultLanguage();
        }

#pragma warning restore S1118 // Utility classes should not have public constructors
    }
}