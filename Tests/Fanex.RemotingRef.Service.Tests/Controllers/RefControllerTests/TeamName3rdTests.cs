namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RefControllerTestBase
    {
        private readonly string _team3rdId;

        public TeamName3rdTests()
            => _team3rdId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetTeamName3rdOfRefDataHelper()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetTeamName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetTeamName3rd(Arg.Is(_team3rdId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetTeamName3rd(Arg.Is(_team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
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

                RefData
                    .GetTeamName3rd(Arg.Is(_team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(teamNameInSpecificLanguage);

                expectedTeamNames.Add(teamNameInSpecificLanguage);
            }

            return expectedTeamNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            Fake3rdTeamNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            Fake3rdTeamNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void Fake3rdTeamNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetTeamName3rd(Arg.Is(_team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}