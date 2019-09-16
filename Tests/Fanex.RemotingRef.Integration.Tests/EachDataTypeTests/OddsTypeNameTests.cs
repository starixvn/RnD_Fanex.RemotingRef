namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class OddsTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing oddsTypeId | GET: expected odds type name")]
        public void GetOddsTypeNameByExistingOddsTypeId()
        {
            // Arrange.
            const string existingOddsTypeId = "1";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(existingOddsTypeId, TestingLanguage);

            // Assert.
            const string expectedOddsTypeName = "Dec";

            Assert.Equal(expectedOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting oddsTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingOddsTypeId()
        {
            // Arrange.
            const string unexistingOddsTypeId = "-1";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(unexistingOddsTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting oddsTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingOddsTypeId()
        {
            // Arrange.
            const string unexistingOddsTypeId = "-2";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                unexistingOddsTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualOddsTypeName);
        }
    }
}