namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RefControllerTestBase
    {
        private readonly string _teamId;

        public TeamNameTests()
            => _teamId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetTeamNameOfRefDataHelper()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetTeamName(Arg.Is(_teamId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetTeamName(Arg.Is(_teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTeamNames();

            // Act.
            var actualResult = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedTeamNames = FakeTeamNames();

            // Act.
            var actualResult = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedTeamNames, actualResult.Values);
        }

        private List<string> FakeTeamNames()
        {
            var expectedTeamNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var teamNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetTeamName(Arg.Is(_teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(teamNameInSpecificLanguage);

                expectedTeamNames.Add(teamNameInSpecificLanguage);
            }

            return expectedTeamNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeTeamNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeTeamNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeTeamNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetTeamName(Arg.Is(_teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}