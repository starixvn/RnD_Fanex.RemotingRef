namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class TeamNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetTeamNameByValidTeamId()
        {
            // Arrange.
            var teamId = "6";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguage);

            // Assert.
            var expectedTeamName = "Arsenal";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid teamId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidTeamId(string teamId)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguage);

            // Assert.
            Assert.Empty(actualTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid teamId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidTeamId(string teamId)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                teamId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualTeamName);
        }
    }
}