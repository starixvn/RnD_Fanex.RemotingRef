namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid lotteryBetChoiceId | GET: expected Lottery bet choice")]
        public void GetLotteryBetChoiceWithValidBetChoiceId()
        {
            // Arrange.
            var lotteryBetChoiceId = "43";

            // Act.
            var actualBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedBetChoice = "Big";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        [Description("GIVE: valid lotteryBetChoiceId | GET: expected Lottery bet type Id")]
        public void GetLotteryBetTypeIdWithValidBetChoiceId()
        {
            // Arrange.
            var lotteryBetChoiceId = "43";

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            var expectedBetTypeId = "17";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet choice")]
        public void GetEmptyStringAsLotteryBetChoiceWithInvalidBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            var actualBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            Assert.Empty(actualBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet type Id")]
        public void GetEmptyStringAsLotteryBetTypeIdWithInvalidBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            Assert.Empty(actualBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid lotteryBetChoiceId | GET: given defaultValue as Lottery bet choice")]
        public void GetGivenDefaultAsBetChoiceValueWithInvalidBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            var actualBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out _);

            // Assert.
            Assert.Equal(DefaultValue, actualBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid lotteryBetChoiceId | GET: given defaultValue as Lottery bet type Id")]
        public void GetGivenDefaultValueAsBetTypeIdWithInvalidBetChoiceId(string lotteryBetChoiceId)
        {
            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out string actualBetTypeId);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeId);
        }
    }
}