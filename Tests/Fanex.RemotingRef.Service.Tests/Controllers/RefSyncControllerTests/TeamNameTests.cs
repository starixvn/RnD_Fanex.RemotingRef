namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingTeams = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _teamIds;

        public TeamNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _teamIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingTeams; counter++)
            {
                var teamId = Fixture.Create<string>();

                _teamIds.Add(teamId);
                _parameterSets.Add(new ParameterSet
                {
                    ["teamId"] = teamId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetTeamNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.TeamName(_formData);

            // Assert.
            RefData.Received().GetTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachTeamId()
        {
            // Act.
            RefSyncController.TeamName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.TeamName(_formData);

            // Assert.
            foreach (var teamId in _teamIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetTeamName(Arg.Is(teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedTeamNames();

            // Act.
            var actualResult = RefSyncController.TeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedTeamNames(true, null);

            // Act.
            var actualResult = RefSyncController.TeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedTeamNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.TeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedTeamNames()
            => FakeSyncedTeamNames(false, null);

        private SyncedTranslationResultSet FakeSyncedTeamNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedTeamNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var teamNameInMultilanguages = new TranslationSet();
                var teamId = _teamIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var teamNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetTeamName(Arg.Is(teamId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(teamNameInSpecificLanguage);
                    teamNameInMultilanguages.Add(standardLanguageCode, teamNameInSpecificLanguage);
                }

                if (ValidateTextSet(teamNameInMultilanguages))
                {
                    expectedSyncedTeamNames.Add(_parameterSets[counter], teamNameInMultilanguages);
                }
            }

            return expectedSyncedTeamNames;
        }
    }
}