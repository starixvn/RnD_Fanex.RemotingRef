namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ResourceDataTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetResourceDataByValidRefIdAndResourceId()
        {
            // Arrange.
            const string validRefId = "AG_SicBo";
            const string validResourceId = "41";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                validRefId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedResource = "Big";

            Assert.Equal(expectedResource, actualResource);
        }

        [Fact]
        public void TakeBackDefaultValueWithInvalidRefId()
        {
            // Arrange.
            const string invalidRefId = "-1";
            const string validResourceId = "41";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                invalidRefId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualResource);
        }

        [Fact]
        public void TakeBackDefaultValueWithInvalidResourceId()
        {
            // Arrange.
            const string validRefId = "AG_SicBo";
            const string invalidResourceId = "-1";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                validRefId,
                invalidResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualResource);
        }
    }
}