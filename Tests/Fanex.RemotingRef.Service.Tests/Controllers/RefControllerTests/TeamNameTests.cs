namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RefControllerBaseTests
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
            RefData.Received().GetTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetTeamName(Arg.Is(_teamId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetTeamName(Arg.Is(_teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
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
    }
}