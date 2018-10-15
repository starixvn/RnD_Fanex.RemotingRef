namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class CombinationTypeComboTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetCombosByValidParameters()
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
        [InlineData("", null, 0, -1)]
        [InlineData(null, "", -1, 0)]
        [InlineData("-1", "0", int.MaxValue, int.MinValue)]
        [InlineData("0", "-1", int.MinValue, int.MaxValue)]
        public void GetNullByInvalidParameters(
            string incorrectBetTeam,
            string incorrectBetCheck,
            int incorrectHandicap1,
            int incorrectHandicap2)
        {
            // Act.
            var actualCombos = RemotingRefService.GetCombinationTypeCombo(
                incorrectBetTeam,
                incorrectBetCheck,
                incorrectHandicap1,
                incorrectHandicap2,
                TestingLanguage);

            // Assert.
            Assert.Null(actualCombos);
        }
    }
}