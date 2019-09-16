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
            const string existingItemId = "3";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(existingItemId, TestingLanguage);

            // Assert.
            const string expectedBetItem = "2";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        [Description("GIVE: (unexisting itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingItemId()
        {
            // Arrange.
            const string unexistingItemId = "-1";

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
            const string unexistingItemId = "-2";

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