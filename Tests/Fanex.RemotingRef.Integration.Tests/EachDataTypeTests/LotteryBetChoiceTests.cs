namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetLotteryBetChoiceByValidLotteryBetChoiceId()
        {
            // Arrange.
            var lotteryBetChoiceId = "43";

            // Act.
            var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedLotteryBetChoice = "Big";

            Assert.Equal(expectedLotteryBetChoice, actualLotteryBetChoice);
        }

        [Fact]
        public void GetLotteryBetTypeIdByValidLotteryBetChoiceId()
        {
            // Arrange.
            var lotteryBetChoiceId = "43";

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualLotteryBetTypeId);

            // Assert.
            var expectedLotteryBetTypeId = "17";

            Assert.Equal(expectedLotteryBetTypeId, actualLotteryBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet choice")]
        public void GetEmptyStringAsLotteryBetChoiceByInvalidLotteryBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            Assert.Empty(actualLotteryBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet type Id")]
        public void GetEmptyStringAsLotteryBetTypeIdWithInvalidLotteryBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Empty(actualLotteryBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid lotteryBetChoiceId | GET: given defaultValue as Lottery bet choice")]
        public void TakeBackDefaultValueAsLotteryBetChoiceWithInvalidLotteryBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out _);

            // Assert.
            Assert.Equal(DefaultValue, actualLotteryBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid lotteryBetChoiceId | GET: given defaultValue as Lottery bet type Id")]
        public void TakeBackDefaultValueAsLotteryBetTypeIdWithInvalidLotteryBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(DefaultValue, actualLotteryBetTypeId);
        }
    }
}