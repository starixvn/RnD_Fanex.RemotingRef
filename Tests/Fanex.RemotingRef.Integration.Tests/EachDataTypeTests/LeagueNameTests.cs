namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing leagueId | GET: expected league name")]
        public void GetLeagueNameByExistingLeagueId()
        {
            // Arrange.
            var existingLeagueId = "13";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(existingLeagueId, TestingLanguage);

            // Assert.
            var expectedLeagueName = "UEFA CHAMPIONS LEAGUE QUALIFIERS";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Fact]
        [Description("GIVE: (unexisting leagueId) && (no default value provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingLeagueId()
        {
            // Arrange.
            var unexistingLeagueId = "-100";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(unexistingLeagueId, TestingLanguage);

            // Assert.
            Assert.Empty(actualLeagueName);
        }

        [Fact]
        [Description("GIVE: unexisting leagueId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingLeagueId()
        {
            // Arrange.
            var unexistingLeagueId = "-100";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                unexistingLeagueId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualLeagueName);
        }
    }
}