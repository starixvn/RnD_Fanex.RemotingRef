namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class FastMarketsResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid resultId | GET: expected Fast Markets result")]
        public void GetFastMarketsResultWithValidResultId()
        {
            // Arrange.
            var resultId = "2";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguage);

            // Assert.
            var expectedResult = "Goal";

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid resultId | GET: empty string")]
        public void GetEmptyStringWithInvalidResultId(string resultId)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}