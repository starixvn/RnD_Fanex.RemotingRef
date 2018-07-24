namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class LeagueName3rdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid leagueId3rd | GET: expected 3rd league name")]
        public void Get3rdLeagueNameWithValidLeagueId3rd()
        {
            // Arrange.
            var leagueId3rd = "1";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            var expectedLeagueName = "KlongHae Songkhla Early";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid leagueId3rd) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidLeagueId3rd(string leagueId3rd)
        {
            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            Assert.Empty(actualLeagueName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-100")]
        [Description("GIVE: invalid leagueId3rd | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidLeagueId3rd(string leagueId3rd)
        {
            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName3rd(
                leagueId3rd,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualLeagueName);
        }
    }
}