namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using NSubstitute;
    using Xunit;

    public class HorseTeamNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingHorseTeams = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _codeIds;

        public HorseTeamNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _codeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingHorseTeams; counter++)
            {
                var codeId = Fixture.Create<string>();

                _codeIds.Add(codeId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(codeId)] = codeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };

            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetHorseTeamNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.HorseTeamName(_formData);

            // Assert.
            RefData
                .Received()
                .GetHorseTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachCodeId()
        {
            // Act.
            RefSyncController.HorseTeamName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetHorseTeamName(Arg.Any<string>(), Arg.Any<string>());
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
                    RefData
                        .Received()
                        .GetHorseTeamName(Arg.Is(codeId), Arg.Is(standardLanguageCode));
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

        private SyncedTranslationResultSet FakeSyncedHorseTeamNames()
            => FakeSyncedHorseTeamNames(false, null);

        private SyncedTranslationResultSet FakeSyncedHorseTeamNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedHorseTeamNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var horseTeamNameInMultilanguages = new TranslationSet();
                var codeId = _codeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var horseTeamNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetHorseTeamName(Arg.Is(codeId), Arg.Is(standardLanguageCode))
                        .Returns(horseTeamNameInSpecificLanguage);

                    horseTeamNameInMultilanguages.Add(standardLanguageCode, horseTeamNameInSpecificLanguage);
                }

                if (ValidateTextSet(horseTeamNameInMultilanguages))
                {
                    expectedSyncedHorseTeamNames.Add(_parameterSets[counter], horseTeamNameInMultilanguages);
                }
            }

            return expectedSyncedHorseTeamNames;
        }
    }
}