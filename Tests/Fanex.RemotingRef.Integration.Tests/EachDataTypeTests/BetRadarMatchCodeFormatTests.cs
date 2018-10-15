namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class BetRadarMatchCodeFormatTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetMatchNameByValidMatchCode()
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
        public void TakeBackMatchCodeWithInvalidMatchCode()
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