namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class SportNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing sportTypeId | GET: expected sport name")]
        public void GetSportNameByExistingSportTypeId()
        {
            // Arrange.
            const string existingSportTypeId = "1";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(existingSportTypeId, TestingLanguage);

            // Assert.
            const string expectedSportName = "Soccer";

            Assert.Equal(expectedSportName, actualSportName);
        }

        [Fact]
        [Description("GIVE: (unexisting sportTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingSportTypeId()
        {
            // Arrange.
            const string unexistingSportTypeId = "-1";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(unexistingSportTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualSportName);
        }

        [Fact]
        [Description("GIVE: unexisting sportTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingSportTypeId()
        {
            // Arrange.
            const string unexistingSportTypeId = "-2";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(
                unexistingSportTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualSportName);
        }
    }
}