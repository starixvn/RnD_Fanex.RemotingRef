namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RefControllerBaseTests
    {
        private readonly string _team3rdId;

        public TeamName3rdTests()
            => _team3rdId = Fixture.Create<string>();

        [Fact]
        public void CallGetTeamName3rdOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetTeamName3rd(Arg.Is(_team3rdId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetTeamName3rdOfRefDataHelper()
        {
            // Act.
            RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetTeamName3rd(Arg.Is(_team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTeam3rdNames();

            // Act.
            var actualTeam3rdNames = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualTeam3rdNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetTeamName3rdOfRefDataHelper()
        {
            // Arrange.
            var expectedTeam3rdNames = FakeTeam3rdNames();

            // Act.
            var actualTeam3rdNames = RefController.TeamName3rd(_team3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedTeam3rdNames, actualTeam3rdNames);
        }

        private Dictionary<string, string> FakeTeam3rdNames()
        {
            var expectedTeam3rdNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var teamNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetTeamName3rd(Arg.Is(_team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(teamNameInSpecificLanguage);
                expectedTeam3rdNames.Add(standardLanguageCode, teamNameInSpecificLanguage);
            }

            return expectedTeam3rdNames;
        }
    }
}