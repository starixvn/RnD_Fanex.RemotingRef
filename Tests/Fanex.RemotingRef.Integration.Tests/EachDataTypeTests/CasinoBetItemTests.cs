namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CasinoBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetCasinoBetItemByValidItemId()
        {
            // Arrange.
            var itemId = "2";

            // Act.
            var actualBetItem = RemotingRefService.GetCasinoBetItem(itemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "Player";

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
            var actualBetItem = RemotingRefService.GetCasinoBetItem(itemId, TestingLanguage);

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
            var actualBetItem = RemotingRefService.GetCasinoBetItem(
                itemId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetItem);
        }
    }
}