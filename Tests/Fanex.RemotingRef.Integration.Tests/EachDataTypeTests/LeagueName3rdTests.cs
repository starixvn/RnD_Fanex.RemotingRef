namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing league3rdId | GET: expected 3rd league name")]
        public void Get3rdLeagueNameByExistingLeague3rdId()
        {
            // Arrange.
            var existingLeague3rdId = "1";

            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(existingLeague3rdId, TestingLanguage);

            // Assert.
            var expected3rdLeagueName = "KlongHae Songkhla Early";

            Assert.Equal(expected3rdLeagueName, actual3rdLeagueName);
        }

        [Fact]
        [Description("GIVE: (unexisting league3rdId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingLeague3rdId()
        {
            // Arrange.
            var unexistingLeague3rdId = "-1";

            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(unexistingLeague3rdId, TestingLanguage);

            // Assert.
            Assert.Empty(actual3rdLeagueName);
        }

        [Fact]
        [Description("GIVE: unexisting league3rdId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingLeague3rdId()
        {
            // Arrange.
            var unexistingLeague3rdId = "-2";

            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(
                unexistingLeague3rdId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actual3rdLeagueName);
        }
    }
}