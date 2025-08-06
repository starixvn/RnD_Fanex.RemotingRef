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
    }
}