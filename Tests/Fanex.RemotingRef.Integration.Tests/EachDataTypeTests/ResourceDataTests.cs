namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ResourceDataTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetResourceDataByValidRefIdAndResourceId()
        {
            // Arrange.
            var validRefId = "AG_SicBo";
            var validResourceId = "41";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                validRefId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedResource = "Big";

            Assert.Equal(expectedResource, actualResource);
        }

        [Fact]
        public void TakeBackDefaultValueWithInvalidRefId()
        {
            // Arrange.
            var invalidRefId = "-1";
            var validResourceId = "41";

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
            var validRefId = "AG_SicBo";
            var invalidResourceId = "-1";

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