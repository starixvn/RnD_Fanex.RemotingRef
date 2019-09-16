namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class TeamNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing teamId | GET: expected team name")]
        public void GetTeamNameByExistingTeamId()
        {
            // Arrange.
            const string existingTeamId = "6";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(existingTeamId, TestingLanguage);

            // Assert.
            const string expectedTeamName = "Arsenal";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        [Description("GIVE: (unexisting teamId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingTeamId()
        {
            // Arrange.
            const string unexistingTeamId = "-1";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(unexistingTeamId, TestingLanguage);

            // Assert.
            Assert.Empty(actualTeamName);
        }

        [Fact]
        [Description("GIVE: unexisting teamId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingTeamId()
        {
            // Arrange.
            const string unexistingTeamId = "-2";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                unexistingTeamId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualTeamName);
        }
    }
}