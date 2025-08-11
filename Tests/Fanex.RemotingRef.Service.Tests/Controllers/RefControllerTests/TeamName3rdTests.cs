namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Controllers;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RefControllerTestBase
    {
        private readonly string _team3rdId;

        public TeamName3rdTests()
        {
            _team3rdId = Fixture.Create<string>();
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetTeamName3rdOfRefDataHelper()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Team3rdService
                .Received()
                .GetTeam3rdById(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Team3rdService
                .Received(StandardLanguageCodes.Length)
                .GetTeam3rdById(Arg.Is(_team3rdId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                Team3rdService
                    .Received()
                    .GetTeam3rdById(Arg.Is(_team3rdId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTeam3rdNames();

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedTeam3rdNames = FakeTeam3rdNames();

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedTeam3rdNames, actualResult.Values);
        }

        private List<string> FakeTeam3rdNames()
        {
            var expectedTeamNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var teamNameInSpecificLanguage = Fixture.Create<string>();

                Team3rdService
                    .GetTeam3rdById(Arg.Is(_team3rdId), Arg.Is( standardLanguageCode))
                    .Returns(teamNameInSpecificLanguage);

                expectedTeamNames.Add(teamNameInSpecificLanguage);
            }

            return expectedTeamNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            Fake3rdTeamNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            Fake3rdTeamNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void Fake3rdTeamNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                Team3rdService
                    .GetTeam3rdById(Arg.Is(_team3rdId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}