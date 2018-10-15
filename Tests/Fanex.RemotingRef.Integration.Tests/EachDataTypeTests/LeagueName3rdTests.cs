namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void Get3rdLeagueNameByValid3rdLeagueId()
        {
            // Arrange.
            var leagueId3rd = "1";

            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            var expected3rdLeagueName = "KlongHae Songkhla Early";

            Assert.Equal(expected3rdLeagueName, actual3rdLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid leagueId3rd) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalid3rdLeagueId(string leagueId3rd)
        {
            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            Assert.Empty(actual3rdLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid leagueId3rd | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalid3rdLeagueId(string leagueId3rd)
        {
            // Act.
            var actual3rdLeagueName = RemotingRefService.GetLeagueName3rd(
                leagueId3rd,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actual3rdLeagueName);
        }
    }
}