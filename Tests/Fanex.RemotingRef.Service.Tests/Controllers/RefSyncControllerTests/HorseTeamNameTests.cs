namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class HorseTeamNameTests : RefSyncControllerBaseTests
    {
        private const int NumberOfTestingHorseTeams = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _codeIds;

        public HorseTeamNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _codeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingHorseTeams; counter++)
            {
                var codeId = Fixture.Create<string>();

                _codeIds.Add(codeId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["codeId"] = codeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetHorseTeamNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.HorseTeamName(_formData);

            // Assert.
            RefData.Received().GetHorseTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachCodeId()
        {
            // Act.
            RefSyncController.HorseTeamName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetHorseTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.HorseTeamName(_formData);

            // Assert.
            foreach (var codeId in _codeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetHorseTeamName(Arg.Is(codeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedHorseTeamNames();

            // Act.
            var actualResult = RefSyncController.HorseTeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedHorseTeamNames(true, null);

            // Act.
            var actualResult = RefSyncController.HorseTeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedHorseTeamNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.HorseTeamName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedHorseTeamNames(
            bool requireSpecialValue = false,
            string specialValue = null)
        {
            var expectedSyncedHorseTeamNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var horseTeamNameInMultilanguages = new Dictionary<string, string>();
                var codeId = _codeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var horseTeamNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetHorseTeamName(Arg.Is(codeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(horseTeamNameInSpecificLanguage);
                    horseTeamNameInMultilanguages.Add(standardLanguageCode, horseTeamNameInSpecificLanguage);
                }

                if (horseTeamNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedHorseTeamNames.Add(_parameterSets[counter], horseTeamNameInMultilanguages);
                }
            }

            return expectedSyncedHorseTeamNames;
        }
    }
}