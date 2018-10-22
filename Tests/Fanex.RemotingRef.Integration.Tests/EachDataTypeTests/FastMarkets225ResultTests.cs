namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarkets225ResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetBetType225ResultByExistingResultId()
        {
            // Arrange.
            var existingBetType255ResultId = "164";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(existingBetType255ResultId, TestingLanguage);

            // Assert.
            var expectedResult = "Penalty, Throw In, Corner";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetEmptyStringByUnexistingResultId()
        {
            // Arrange.
            var unexistingBetType255ResultId = "-1";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(unexistingBetType255ResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}