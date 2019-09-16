namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid match code pattern | GET: full match name")]
        public void GetMatchNameByValidMatchCode()
        {
            // Arrange.
            const string validMatchCode = "123 {8}";

            // Act.
            var actualMatch = RemotingRefService.GetBetRadarMatchCodeFormat(validMatchCode, TestingLanguage);

            // Assert.
            const string expectedMatch = "123 Match Day 2 Groups E&F";

            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        [Description("GIVE: invalid match code pattern | GET: given match code")]
        public void TakeBackMatchCodeWithInvalidMatchCode()
        {
            // Arrange.
            const string invalidMatchCode = "123";

            // Act.
            var actualMatchName = RemotingRefService.GetBetRadarMatchCodeFormat(invalidMatchCode, TestingLanguage);

            // Assert.
            Assert.Equal(invalidMatchCode, actualMatchName);
        }
    }
}