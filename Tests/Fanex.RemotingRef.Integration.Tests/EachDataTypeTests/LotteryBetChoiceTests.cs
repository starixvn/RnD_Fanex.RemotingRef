namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LotteryBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing lotteryBetChoiceId | GET: expected Lottery bet choice")]
        public void GetLotteryBetChoiceByExistingLotteryBetChoiceId()
        {
            // Arrange.
            const string existingLotteryBetChoiceId = "43";

            // Act.
            var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
                existingLotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            const string expectedLotteryBetChoice = "Big";

            Assert.Equal(expectedLotteryBetChoice, actualLotteryBetChoice);
        }

        [Fact]
        [Description("GIVE: existing lotteryBetChoiceId | GET: expected Lottery bet type ID")]
        public void GetLotteryBetTypeIdByExistingLotteryBetChoiceId()
        {
            // Arrange.
            const string existingLotteryBetChoiceId = "43";

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                existingLotteryBetChoiceId,
                TestingLanguage,
                out string actualLotteryBetTypeId);

            // Assert.
            const string expectedLotteryBetTypeId = "17";

            Assert.Equal(expectedLotteryBetTypeId, actualLotteryBetTypeId);
        }

        //[Fact]
        //[Description("GIVE: (unexisting lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet choice")]
        //public void GetEmptyStringAsLotteryBetChoiceByUnexistingLotteryBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingLotteryBetChoiceId = "-1";

        //    // Act.
        //    var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
        //        unexistingLotteryBetChoiceId,
        //        TestingLanguage,
        //        out _);

        //    // Assert.
        //    Assert.Empty(actualLotteryBetChoice);
        //}

        //[Fact]
        //[Description("GIVE: (unexisting lotteryBetChoiceId) && (no defaultValue provided) | GET: empty string as Lottery bet type ID")]
        //public void GetEmptyStringAsLotteryBetTypeIdByUnexistingLotteryBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingLotteryBetChoiceId = "-1";

        //    // Act.
        //    RemotingRefService.GetLotteryBetChoice(
        //        unexistingLotteryBetChoiceId,
        //        TestingLanguage,
        //        out string actualLotteryBetTypeId);

        //    // Assert.
        //    Assert.Empty(actualLotteryBetTypeId);
        //}

        //[Fact]
        //[Description("GIVE: unexisting lotteryBetChoiceId | GET: given defaultValue as Lottery bet choice")]
        //public void TakeBackDefaultValueAsLotteryBetChoiceWithUnexistingLotteryBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingLotteryBetChoiceId = "-2";

        //    // Act.
        //    var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
        //        unexistingLotteryBetChoiceId,
        //        TestingLanguage,
        //        DefaultValue,
        //        out _);

        //    // Assert.
        //    Assert.Equal(DefaultValue, actualLotteryBetChoice);
        //}

        //[Fact]
        //[Description("GIVE: unexisting lotteryBetChoiceId | GET: given defaultValue as Lottery bet type ID")]
        //public void TakeBackDefaultValueAsLotteryBetTypeIdWithUnexistingLotteryBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingLotteryBetChoiceId = "-2";

        //    // Act.
        //    RemotingRefService.GetLotteryBetChoice(
        //        unexistingLotteryBetChoiceId,
        //        TestingLanguage,
        //        DefaultValue,
        //        out string actualLotteryBetTypeId);

        //    // Assert.
        //    Assert.Equal(DefaultValue, actualLotteryBetTypeId);
        //}
    }
}