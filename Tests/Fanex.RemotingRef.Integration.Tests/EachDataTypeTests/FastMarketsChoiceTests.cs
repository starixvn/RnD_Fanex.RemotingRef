namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
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

        [Theory]
        [InlineData("")]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (betTypeId is not null) && (invalid betTypeId) | GET: string only contains time interval")]
        public void GetTimeIntervalOnlyByNonNullInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var betTeam = "1";
            var handicap1 = 1;
            var handicap2 = 1;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = " (01:00-01:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetEmptyStringByNullBetTypeId()
        {
            // Arrange.
            var betTypeId = (string)null;
            var betTeam = "2";
            var handicap1 = 3;
            var handicap2 = 4;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            Assert.Empty(actualChoice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid betTeam | GET: string only contains time interval")]
        public void GetTimeIntervalOnlyByInvalidBetTeamId(string betTeam)
        {
            // Arrange.
            var betTypeId = "223";
            var handicap1 = 5;
            var handicap2 = 6;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = " (05:00-05:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }
    }
}