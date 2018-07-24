namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CombinationTypeComboTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: (valid betCheck) && (valid betTeam) && (valid handicap1) && (valid handicap2) | GET: expected combos")]
        public void GetCombosWithValidParameters()
        {
            // Arrange.
            var betTeam = "110";
            var betCheck = "2";
            var handicap1 = 5;
            var handicap2 = 10;

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

        [Theory]
        [InlineData("", 0)]
        [InlineData(null, -1)]
        [InlineData("-1", int.MaxValue)]
        [Description("GIVE: invalid parameters | GET: null")]
        public void GetNullWithInvalidParameters(string incorrectStringValue, int incorrectIntegerValue)
        {
            // Arrange.
            var betTeam = incorrectStringValue;
            var betCheck = incorrectStringValue;
            var handicap1 = incorrectIntegerValue;
            var handicap2 = incorrectIntegerValue;

            // Act.
            var actualCombos = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            Assert.Null(actualCombos);
        }
    }
}