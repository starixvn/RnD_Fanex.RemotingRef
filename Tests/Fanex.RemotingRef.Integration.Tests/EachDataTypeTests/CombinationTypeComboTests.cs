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
            var betTeam = string.Empty;
            var betCheck = string.Empty;
            var handicap1 = 0;
            var handicap2 = 21;

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
            var betTeam = string.Empty;
            var betCheck = "1026";
            var handicap1 = 1;
            var handicap2 = 12;

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
            var betTeam = "110";
            var betCheck = string.Empty;
            var handicap1 = 0;
            var handicap2 = 0;

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