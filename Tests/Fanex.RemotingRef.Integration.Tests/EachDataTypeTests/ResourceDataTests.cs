namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ResourceDataTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetResourceDataByValidRefIdAndResourceId()
        {
            // Arrange.
            var refId = "AG_SicBo";
            var resourceId = "41";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedResource = "Big";

            Assert.Equal(expectedResource, actualResource);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        public void TakeBackDefaultValueWithInvalidRefId(string refId)
        {
            // Arrange.
            var resourceId = "41";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualResource);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        public void TakeBackDefaultValueWithInvalidResourceId(string resourceId)
        {
            // Arrange.
            var refId = "AG_SicBo";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualResource);
        }
    }
}