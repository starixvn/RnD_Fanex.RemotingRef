namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class FastMarkets225ResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetBetType225ResultByValidResultId()
        {
            // Arrange.
            var betType255ResultId = "164";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguage);

            // Assert.
            var expectedResult = "Penalty, Throw In, Corner";

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        public void GetEmptyStringByInvalidResultId(string betType255ResultId)
        {
            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}