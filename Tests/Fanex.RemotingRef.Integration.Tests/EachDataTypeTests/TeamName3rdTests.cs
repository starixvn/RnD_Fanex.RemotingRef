namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class TeamName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing team3rdId | GET: expected 3rd team name")]
        public void Get3rdTeamNameByExistingTeam3rdId()
        {
            // Arrange.
            const string existingTeam3rdId = "1";

            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName3rd(existingTeam3rdId, TestingLanguage);

            // Assert.
            const string expected3rdTeamName = "Langsad SriHaYuth (DanYai)";

            Assert.Equal(expected3rdTeamName, actual3rdTeamName);
        }

        [Fact]
        [Description("GIVE: (unexisting team3rdId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingTeam3rdId()
        {
            // Arrange.
            const string unexistingTeam3rdId = "-1";

            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName3rd(unexistingTeam3rdId, TestingLanguage);

            // Assert.
            Assert.Empty(actual3rdTeamName);
        }

        [Fact]
        [Description("GIVE: unexisting team3rdId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingTeam3rdId()
        {
            // Arrange.
            const string unexistingTeam3rdId = "-2";

            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName3rd(
                unexistingTeam3rdId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actual3rdTeamName);
        }
    }
}