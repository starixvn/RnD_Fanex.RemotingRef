namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class SportNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetSportNameByValidSportTypeId()
        {
            // Arrange.
            var sportTypeId = "1";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguage);

            // Assert.
            var expectedSportName = "Soccer";

            Assert.Equal(expectedSportName, actualSportName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid sportTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidSportTypeId(string sportTypeId)
        {
            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualSportName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid sportTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidSportTypeId(string sportTypeId)
        {
            // Act.
            var actualSportName = RemotingRefService.GetSportName(
                sportTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualSportName);
        }
    }
}