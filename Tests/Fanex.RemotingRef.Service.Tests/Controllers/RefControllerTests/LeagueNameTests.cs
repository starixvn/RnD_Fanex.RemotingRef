namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RefControllerBaseTests
    {
        private readonly string _leagueId;

        public LeagueNameTests()
            => _leagueId = Fixture.Create<string>();

        [Fact]
        public void CallGetLeagueNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLeagueName(Arg.Is(_leagueId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetLeagueNameOfRefDataHelper()
        {
            // Act.
            RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetLeagueName(Arg.Is(_leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLeagueNames();

            // Act.
            var actualLeagueNames = RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualLeagueNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetLeagueNameOfRefDataHelper()
        {
            // Arrange.
            var expectedLeagueNames = FakeLeagueNames();

            // Act.
            var actualLeagueNames = RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLeagueNames, actualLeagueNames);
        }

        private Dictionary<string, string> FakeLeagueNames()
        {
            var expectedLeagueNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var leagueNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLeagueName(Arg.Is(_leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(leagueNameInSpecificLanguage);
                expectedLeagueNames.Add(standardLanguageCode, leagueNameInSpecificLanguage);
            }

            return expectedLeagueNames;
        }
    }
}