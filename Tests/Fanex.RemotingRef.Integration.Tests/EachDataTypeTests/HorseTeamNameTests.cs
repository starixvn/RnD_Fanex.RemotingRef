namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class HorseTeamNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing codeId | GET: expected horse team name")]
        public void GetHorseTeamNameByExistingCodeId()
        {
            // Arrange.
            var existingCodeId = "53438";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(existingCodeId, TestingLanguage);

            // Assert.
            var expectedHorseTeamName = "T Keddy";

            Assert.Equal(expectedHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        [Description("GIVE: (unexisting codeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingCodeId()
        {
            // Arrange.
            var unexistingCodeId = "-1";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(unexistingCodeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }

        [Fact]
        [Description("GIVE: unexisting codeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingCodeId()
        {
            // Arrange.
            var unexistingCodeId = "-2";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(
                unexistingCodeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualHorseTeamName);
        }
    }
}