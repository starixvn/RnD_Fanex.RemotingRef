namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid matchCode | GET: expected match name")]
        public void GetMatchNameWithValidMatchCode()
        {
            // Arrange.
            var matchCode = "123 {8}";

            // Act.
            var actualMatch = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguage);

            // Assert.
            var expectedMatch = "123 Match Day 2 Groups E&F";

            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        [Description("GIVE: invalid teamId | GET: empty string")]
        public void GetGivenMatchCodeIfNoDataFound()
        {
            // Arrange.
            var matchCode = "123";

            // Act.
            var actualMatchName = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguage);

            // Assert.
            Assert.Equal(matchCode, actualMatchName);
        }
    }
}