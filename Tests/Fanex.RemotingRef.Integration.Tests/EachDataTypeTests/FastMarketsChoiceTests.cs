namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarketsChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetFastMarketChoiceByValidParameters()
        {
            // Arrange.
            const string betTypeId = "223";
            const string betTeam = "1";
            const int handicap1 = 1;
            const int handicap2 = 2;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            const string expectedChoice = "Goal (01:00-01:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetTimeIntervalOnlyByUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-1";
            const string betTeam = "1";
            const int handicap1 = 1;
            const int handicap2 = 1;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                unexistingBetTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            const string expectedChoice = " (01:00-01:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetTimeIntervalOnlyByIncorrectBetTeamId()
        {
            // Arrange.
            const string betTypeId = "223";
            const string incorrectBetTeam = "-1";
            const int handicap1 = 5;
            const int handicap2 = 6;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                incorrectBetTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            const string expectedChoice = " (05:00-05:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }
    }
}