namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class HorseTeamNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetHorseTeamNameByValidCodeId()
        {
            // Arrange.
            var codeId = "53438";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguage);

            // Assert.
            var expectedHorseTeamName = "T Keddy";

            Assert.Equal(expectedHorseTeamName, actualHorseTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid codeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidCodeId(string codeId)
        {
            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualHorseTeamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid codeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidCodeId(string codeId)
        {
            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(
                codeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualHorseTeamName);
        }
    }
}