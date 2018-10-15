namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class RouletteBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetRouletteBetItemByValidItemId()
        {
            // Arrange.
            var itemId = "3";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "2";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidItemId(string itemId)
        {
            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetItem);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid itemId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidItemId(string itemId)
        {
            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(
                itemId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetItem);
        }
    }
}