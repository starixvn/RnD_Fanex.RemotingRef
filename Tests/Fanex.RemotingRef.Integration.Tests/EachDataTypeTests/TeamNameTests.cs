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
            var existingTeamId = "6";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(existingTeamId, TestingLanguage);

            // Assert.
            var expectedTeamName = "Arsenal";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        [Description("GIVE: (unexisting teamId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingTeamId()
        {
            // Arrange.
            var unexistingTeamId = "-1";

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
            var unexistingTeamId = "-2";

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