namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid lotteryBetTypeId | GET: expected Lottery bet type name")]
        public void GetLotteryBetTypeNameWithValidLotteryBetTypeId()
        {
            // Arrange.
            var lotteryBetTypeId = "2";

            // Act.
            var actualBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "1st Digit Odd/Even";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid lotteryBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidLotteryBetTypeId(string lotteryBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid lotteryBetTypeId | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidLotteryBetTypeId(string lotteryBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetLotteryBetType(
                lotteryBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }
    }
}