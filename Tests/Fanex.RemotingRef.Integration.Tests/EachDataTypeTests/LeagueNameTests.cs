namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetLeagueNameByValidLeagueId()
        {
            // Arrange.
            var leagueId = "13";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguage);

            // Assert.
            var expectedLeagueName = "UEFA CHAMPIONS LEAGUE QUALIFIERS";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-100")]
        [InlineData("0")]
        [Description("GIVE: (invalid leagueId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidLeagueId(string leagueId)
        {
            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguage);

            // Assert.
            Assert.Empty(actualLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-100")]
        [InlineData("0")]
        [Description("GIVE: invalid leagueId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidLeagueId(string leagueId)
        {
            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                leagueId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualLeagueName);
        }
    }
}