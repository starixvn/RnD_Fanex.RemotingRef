namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CasinoBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid itemId | GET: expected bet item")]
        public void GetCasinoBetItemWithValidItemId()
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
        [Description("GIVE: (invalid itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidItemId(string itemId)
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
        [Description("GIVE: invalid itemId | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidItemId(string itemId)
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