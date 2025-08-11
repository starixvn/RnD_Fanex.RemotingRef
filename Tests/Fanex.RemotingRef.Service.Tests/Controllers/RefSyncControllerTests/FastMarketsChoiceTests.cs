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

    public class FastMarketsChoiceTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingChoices = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<Tuple<string, string, int, int>> _parameterSetsValues;

        public FastMarketsChoiceTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _parameterSetsValues = new List<Tuple<string, string, int, int>>();

            for (var counter = 0; counter < NumberOfTestingChoices; counter++)
            {
                var betTypeId = Fixture.Create<string>();
                var betTeam = Fixture.Create<string>();
                var handicap1 = Fixture.Create<int>();
                var handicap2 = Fixture.Create<int>();

                _parameterSets.Add(new ParameterSet
                {
                    [nameof(betTypeId)] = betTypeId,
                    [nameof(betTeam)] = betTeam,
                    [nameof(handicap1)] = handicap1,
                    [nameof(handicap2)] = handicap2
                });
                _parameterSetsValues.Add(new Tuple<string, string, int, int>(
                    betTypeId,
                    betTeam,
                    handicap1,
                    handicap2));
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
        public void AlwaysCallGetFastMarketsChoiceOfRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            RefData
                .Received()
                .GetFastMarketsChoice(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachParameterSet()
        {
            // Act.
            RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetFastMarketsChoice(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            foreach (var parameterSetsValue in _parameterSetsValues)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetFastMarketsChoice(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(parameterSetsValue.Item4),
                            Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsChoices();

            // Act.
            var actualResult = RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsChoices(true, null);

            // Act.
            var actualResult = RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsChoices(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.FastMarketsChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedFastMarketsChoices()
            => FakeSyncedFastMarketsChoices(false, null);

        private SyncedTranslationResultSet FakeSyncedFastMarketsChoices(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedFastMartketsChoices = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 0; counter < _parameterSets.Count; counter++)
            {
                var choiceNameInMultilanguages = new TranslationSet();
                var parameterSetsValue = _parameterSetsValues[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var choiceNameInSpecificLanguage = specialIndex == counter ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetFastMarketsChoice(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(parameterSetsValue.Item4),
                            Arg.Is(standardLanguageCode))
                        .Returns(choiceNameInSpecificLanguage);

                    choiceNameInMultilanguages.Add(standardLanguageCode, choiceNameInSpecificLanguage);
                }

                if (ValidateTextSet(choiceNameInMultilanguages))
                {
                    expectedSyncedFastMartketsChoices.Add(_parameterSets[counter], choiceNameInMultilanguages);
                }
            }

            return expectedSyncedFastMartketsChoices;
        }
    }
}