namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CombinationTypeComboTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: handicap2 >= 21 | GET: <<handicap2>>-Fold (1 Bet)")]
        public void GetMixParlayCombinatioTypeByHandicap2()
        {
            // Arrange.
            const int handicap1 = 0;
            const int handicap2 = 21;

            var betTeam = string.Empty;
            var betCheck = string.Empty;

            // Act.
            var actualCombinationType = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedCombos = new[] { "21-Fold (1 Bet)" };

            Assert.Equal(expectedCombos, actualCombinationType);
        }

        [Fact]
        [Description("GIVE: (handicap1 == 1) && (betCheck) |"
                   + "GET: list of Combination types in RefResource which bitmap in betCheck meets ResourceId, and RefId is 'CombinationType20dn")]
        public void GetComboParlayCombinationTypsByHandicap1AndSatisfyBetCheck()
        {
            // Arrange.
            const string betCheck = "1026";
            const int handicap1 = 1;
            const int handicap2 = 12;

            var betTeam = string.Empty;

            // Act.
            var actualCombos = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedCombos = new[] { "Doubles (66 Bets)", "11-Folds (12 Bets)" };

            Assert.Equal(expectedCombos, actualCombos);
        }

        [Fact]
        [Description("GIVE: betTeam | GET: Combination type stored in RefResource which ReourceId is betTeam and RefId is 'CombinationType'")]
        public void GetOldSingleCombinationTypeByBetTeam()
        {
            // Arrange.
            const string betTeam = "110";
            const int handicap1 = 0;
            const int handicap2 = 0;

            var betCheck = string.Empty;

            // Act.
            var actualCombos = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedCombos = new[] { "Trixie" };

            Assert.Equal(expectedCombos, actualCombos);
        }
    }
}