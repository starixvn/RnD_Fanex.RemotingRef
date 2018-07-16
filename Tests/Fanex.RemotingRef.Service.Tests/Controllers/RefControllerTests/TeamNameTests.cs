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
        public void CallGetTeamNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetTeamName(Arg.Is(_teamId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetTeamNameOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTeamNames();

            // Act.
            var actualTeamNames = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualTeamNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetTeamNameOfRefDataHelper()
        {
            // Arrange.
            var expectedTeamNames = FakeTeamNames();

            // Act.
            var actualTeamNames = RefController.TeamName(_teamId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedTeamNames, actualTeamNames);
        }

        private Dictionary<string, string> FakeTeamNames()
        {
            var expectedTeamNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var teamNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetTeamName(Arg.Is(_teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(teamNameInSpecificLanguage);
                expectedTeamNames.Add(standardLanguageCode, teamNameInSpecificLanguage);
            }

            return expectedTeamNames;
        }
    }
}