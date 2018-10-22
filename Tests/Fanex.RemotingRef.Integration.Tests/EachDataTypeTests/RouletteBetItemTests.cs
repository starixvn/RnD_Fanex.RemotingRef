namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class RouletteBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing itemId | GET: expected Roulette bet item")]
        public void GetRouletteBetItemByExistingItemId()
        {
            // Arrange.
            var existingItemId = "3";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(existingItemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "2";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        [Description("GIVE: (unexisting itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingItemId()
        {
            // Arrange.
            var unexistingItemId = "-1";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(unexistingItemId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetItem);
        }

        [Fact]
        [Description("GIVE: unexisting itemId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingItemId()
        {
            // Arrange.
            var unexistingItemId = "-2";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(
                unexistingItemId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetItem);
        }
    }
}