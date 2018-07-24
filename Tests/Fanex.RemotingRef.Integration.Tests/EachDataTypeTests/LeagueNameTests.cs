namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid leagueId | GET: expected league name")]
        public void GetLeagueNameWithValidLeagueId()
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
        [Description("GIVE: (invalid leagueId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidLeagueId(string leagueId)
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
        [Description("GIVE: invalid leagueId | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidLeagueId(string leagueId)
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