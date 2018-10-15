namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class FastMarketsResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetFastMarketsResultByValidResultId()
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
        [InlineData("0")]
        public void GetEmptyStringByInvalidResultId(string resultId)
        {
            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}