namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarketsResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetFastMarketsResultByExistingResultId()
        {
            // Arrange.
            var existingResultId = "2";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(existingResultId, TestingLanguage);

            // Assert.
            var expectedResult = "Goal";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetEmptyStringByUnexistingResultId()
        {
            // Arrange.
            var unexistingResultId = "-1";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(unexistingResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}