namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class OddsTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetOddsTypeNameByValidOddsTypeId()
        {
            // Arrange.
            var oddsTypeId = "1";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguage);

            // Assert.
            var expectedOddsTypeName = "Dec";

            Assert.Equal(expectedOddsTypeName, actualOddsTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid oddsTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidOddsTypeId(string oddsTypeId)
        {
            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid oddsTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidOddsTypeId(string oddsTypeId)
        {
            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                oddsTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualOddsTypeName);
        }
    }
}