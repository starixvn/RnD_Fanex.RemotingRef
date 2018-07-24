namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    [CollectionDefinition(nameof(ServiceIntegrationCollection))]
    public class ServiceIntegrationCollection : ICollectionFixture<ServiceIntegrationFixture>
    {
    }
}