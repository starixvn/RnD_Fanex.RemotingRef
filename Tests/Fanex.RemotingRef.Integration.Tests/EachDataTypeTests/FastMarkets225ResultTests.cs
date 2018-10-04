namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class FastMarkets225ResultTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid resultId | GET: expected Fast Markets result")]
        public void GetFastMarkets225ResultWithValidResultId()
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
        [Description("GIVE: invalid resultId | GET: empty string")]
        public void GetEmptyStringWithInvalidResultId(string betType255ResultId)
        {
            // Act.
            var actualTeamName = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguage);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}