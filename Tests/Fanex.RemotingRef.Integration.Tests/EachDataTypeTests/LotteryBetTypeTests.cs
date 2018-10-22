namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing lotteryBetTypeId | GET: expected Lottery bet type name")]
        public void GetLotteryBetTypeNameByExistingLotteryBetTypeId()
        {
            // Arrange.
            var existingLotteryBetTypeId = "2";

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(existingLotteryBetTypeId, TestingLanguage);

            // Assert.
            var expectedLotteryBetTypeName = "1st Digit Odd/Even";

            Assert.Equal(expectedLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting lotteryBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingLotteryBetTypeId()
        {
            // Arrange.
            var unexistingLotteryBetTypeId = "-1";

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(unexistingLotteryBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualLotteryBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting lotteryBetTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingLotteryBetTypeId()
        {
            // Arrange.
            var unexistingLotteryBetTypeId = "-2";

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                unexistingLotteryBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualLotteryBetTypeName);
        }
    }
}