namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class RouletteBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid itemId | GET: expected roulette bet item")]
        public void GetRouletteBetItemWithValidItemId()
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
        [Description("GIVE: (invalid itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidItemId(string itemId)
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
        [Description("GIVE: invalid itemId | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidItemId(string itemId)
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