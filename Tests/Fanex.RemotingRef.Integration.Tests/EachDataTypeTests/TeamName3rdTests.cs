namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class TeamName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid teamId3rd | GET: expected 3rd team name")]
        public void Get3rdTeamNameWithValidTeamId3rd()
        {
            // Arrange.
            var teamId3rd = "1";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName3rd(teamId3rd, TestingLanguage);

            // Assert.
            var expectedTeamName = "Langsad SriHaYuth (DanYai)";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid teamId3rd) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidTeamId3rd(string teamId3rd)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId3rd, TestingLanguage);

            // Assert.
            Assert.Empty(actualTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid teamId3rd | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidTeamId3rd(string teamId3rd)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                teamId3rd,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualTeamName);
        }
    }
}