namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarketsChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetFastMarketChoiceByValidParameters()
        {
            // Arrange.
            var betTypeId = "223";
            var betTeam = "1";
            var handicap1 = 1;
            var handicap2 = 2;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = "Goal (01:00-01:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetTimeIntervalOnlyByUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-1";
            var betTeam = "1";
            var handicap1 = 1;
            var handicap2 = 1;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                unexistingBetTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = " (01:00-01:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetTimeIntervalOnlyByIncorrectBetTeamId()
        {
            // Arrange.
            var betTypeId = "223";
            var incorrectBetTeam = "-1";
            var handicap1 = 5;
            var handicap2 = 6;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                incorrectBetTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = " (05:00-05:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }
    }
}