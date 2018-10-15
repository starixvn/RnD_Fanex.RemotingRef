namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetLotteryBetTypeNameByValidLotteryBetTypeId()
        {
            // Arrange.
            var lotteryBetTypeId = "2";

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            var expectedLotteryBetTypeName = "1st Digit Odd/Even";

            Assert.Equal(expectedLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid lotteryBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidLotteryBetTypeId(string lotteryBetTypeId)
        {
            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualLotteryBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid lotteryBetTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidLotteryBetTypeId(string lotteryBetTypeId)
        {
            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                lotteryBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualLotteryBetTypeName);
        }
    }
}