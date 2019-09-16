namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarketsResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetFastMarketsResultByExistingResultId()
        {
            // Arrange.
            const string existingResultId = "2";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(existingResultId, TestingLanguage);

            // Assert.
            const string expectedResult = "Goal";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetEmptyStringByUnexistingResultId()
        {
            // Arrange.
            const string unexistingResultId = "-1";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(unexistingResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}