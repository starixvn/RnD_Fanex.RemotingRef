namespace Fanex.RemotingRef.Integration.Tests
{
    using System;
    using System.Configuration;

    public class ServiceIntegrationFixture
    {
#pragma warning disable S1118 // Utility classes should not have public constructors

        public ServiceIntegrationFixture()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri("https://remotingref-api-core.nexdev.net/"))
                .FromApplication("UnitTest_AppName")
                .SupportOnlyDefaultLanguage();
        }

#pragma warning restore S1118 // Utility classes should not have public constructors
    }
}