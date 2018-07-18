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
        public void AlwaysCallGetLeagueNameOfRefDataHelper()
        {
            // Act.
            RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            RefData.Received().GetLeagueName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLeagueName(Arg.Is(_leagueId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
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
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLeagueNames();

            // Act.
            var actualResult = RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLeagueNames = FakeLeagueNames();

            // Act.
            var actualResult = RefController.LeagueName(_leagueId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLeagueNames, actualResult.Values);
        }

        private List<string> FakeLeagueNames()
        {
            var expectedLeagueNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var leagueNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLeagueName(Arg.Is(_leagueId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(leagueNameInSpecificLanguage);
                expectedLeagueNames.Add(leagueNameInSpecificLanguage);
            }

            return expectedLeagueNames;
        }
    }
}