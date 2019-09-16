namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarkets225ResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetBetType225ResultByExistingResultId()
        {
            // Arrange.
            const string existingBetType255ResultId = "164";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(existingBetType255ResultId, TestingLanguage);

            // Assert.
            const string expectedResult = "Penalty, Throw In, Corner";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetEmptyStringByUnexistingResultId()
        {
            // Arrange.
            const string unexistingBetType255ResultId = "-1";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(unexistingBetType255ResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}