namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CasinoBetItemTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing itemId | GET: expected casino item")]
        public void GetCasinoBetItemByExistingItemId()
        {
            // Arrange.
            var existingItemId = "2";

            // Act.
            var actualBetItem = RemotingRefService.GetCasinoBetItem(existingItemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "Player";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        [Description("GIVE: (unexisting itemId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingItemId()
        {
            // Arrange.
            var unexistingItemId = "-1";

            // Act.
            var actualBetItem = RemotingRefService.GetCasinoBetItem(unexistingItemId, TestingLanguage);

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
            var actualBetItem = RemotingRefService.GetCasinoBetItem(
                unexistingItemId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetItem);
        }
    }
}