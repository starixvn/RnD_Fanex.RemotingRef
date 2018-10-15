namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class TeamName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void Get3rdTeamNameByValid3rdTeamId()
        {
            // Arrange.
            var teamId3rd = "1";

            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName3rd(teamId3rd, TestingLanguage);

            // Assert.
            var expected3rdTeamName = "Langsad SriHaYuth (DanYai)";

            Assert.Equal(expected3rdTeamName, actual3rdTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid teamId3rd) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalid3rdTeamId(string teamId3rd)
        {
            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName(teamId3rd, TestingLanguage);

            // Assert.
            Assert.Empty(actual3rdTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid teamId3rd | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalid3rdTeamId(string teamId3rd)
        {
            // Act.
            var actual3rdTeamName = RemotingRefService.GetTeamName(
                teamId3rd,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actual3rdTeamName);
        }
    }
}