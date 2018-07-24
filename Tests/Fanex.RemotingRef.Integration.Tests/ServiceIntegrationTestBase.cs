namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    [Collection(nameof(ServiceIntegrationCollection))]
    public class ServiceIntegrationTestBase
    {
        protected const string TestingLanguage = RefDataTypeHandlerOptions.DefaultLanguage;
        protected const string DefaultValue = "defaultValue";

        protected ServiceIntegrationTestBase()
            => RemotingRefService = new RemotingRefService();

        protected IRemotingRefService RemotingRefService { get; }
    }
}