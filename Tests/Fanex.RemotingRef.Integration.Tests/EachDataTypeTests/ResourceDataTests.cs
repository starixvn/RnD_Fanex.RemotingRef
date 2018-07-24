namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class ResourceDataTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: (valid refId) && (valid resourceId) | GET: expected resource data")]
        public void GetResourceDataWithValidRefIdAndResourceId()
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
        [Description("GIVE: invalid refId | GET: given defaultValue")]
        public void GetDefaultValueWithInvalidRefId(string refId)
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
        [Description("GIVE: invalid resourceId | GET: given defaultValue")]
        public void GetDefaultValueWithInvalidResourceId(string resourceId)
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