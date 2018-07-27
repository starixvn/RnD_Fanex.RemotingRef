namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingTeam3rds = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _team3rdIds;

        public TeamName3rdTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _team3rdIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingTeam3rds; counter++)
            {
                var team3rdId = Fixture.Create<string>();

                _team3rdIds.Add(team3rdId);
                _parameterSets.Add(new ParameterSet
                {
                    ["team3rdId"] = team3rdId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetTeamName3rdOfRefDataHelper()
        {
            // Act.
            RefSyncController.TeamName3rd(_formData);

            // Assert.
            RefData.Received().GetTeamName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachTeam3rdId()
        {
            // Act.
            RefSyncController.TeamName3rd(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetTeamName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.TeamName3rd(_formData);

            // Assert.
            foreach (var team3rdId in _team3rdIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetTeamName3rd(Arg.Is(team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdTeamNames();

            // Act.
            var actualResult = RefSyncController.TeamName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdTeamNames(true, null);

            // Act.
            var actualResult = RefSyncController.TeamName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSynced3rdTeamNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.TeamName3rd(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSynced3rdTeamNames()
            => FakeSynced3rdTeamNames(false, null);

        private SyncedTranslationResultSet FakeSynced3rdTeamNames(
            bool requireSpecialValue,
            string specialValue)
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
                var team3rdId = _team3rdIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var teamNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetTeamName3rd(Arg.Is(team3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
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