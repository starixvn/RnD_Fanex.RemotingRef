namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class CombinationTypeComboParlayTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingCombos = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<Tuple<string, string, int, int>> _parameterSetsValues;

        public CombinationTypeComboParlayTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _parameterSetsValues = new List<Tuple<string, string, int, int>>();

            for (var counter = 0; counter < NumberOfTestingCombos; counter++)
            {
                var betTeam = Fixture.Create<string>();
                var betCheck = Fixture.Create<string>();
                var handicap1 = Fixture.Create<int>();
                var handicap2 = Fixture.Create<int>();

                _parameterSets.Add(new ParameterSet
                {
                    [nameof(betTeam)] = betTeam,
                    [nameof(betCheck)] = betCheck,
                    [nameof(handicap1)] = handicap1,
                    [nameof(handicap2)] = handicap2
                });
                _parameterSetsValues.Add(new Tuple<string, string, int, int>(
                    betTeam,
                    betCheck,
                    handicap1,
                    handicap2));
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetCombinationTypeComboOfRefDataHelper()
        {
            // Act.
            RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            RefData
                .Received()
                .GetCombinationTypeCombo(
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
            RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetCombinationTypeCombo(
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
            RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            foreach (var parameterSetsValue in _parameterSetsValues)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetCombinationTypeCombo(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(parameterSetsValue.Item4),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedCombinationTypeComboParlays();

            // Act.
            var actualResult = RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCombinationTypeComboParlays(true, null);

            // Act.
            var actualResult = RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyArrayIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCombinationTypeComboParlays(true, Array.Empty<string>());

            // Act.
            var actualResult = RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ArrayOfEmptyStringsIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCombinationTypeComboParlays(true, new[] { string.Empty });

            // Act.
            var actualResult = RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ArrayOfNullsIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCombinationTypeComboParlays(true, new string[] { null });

            // Act.
            var actualResult = RefSyncController.CombinationTypeComboParlay(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedCombinationTypeComboParlays()
            => FakeSyncedCombinationTypeComboParlays(false, null);

        private SyncedTranslationResultSet FakeSyncedCombinationTypeComboParlays(bool requireSpecialValue, string[] specialValue)
        {
            var expectedSyncedComboNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 0; counter < _parameterSets.Count; counter++)
            {
                var comboNamesInMultilanguages = new TranslationSet();
                var parameterSetsValue = _parameterSetsValues[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var comboNamesInSpecificLanguage = specialIndex == counter ? specialValue : new[] { Fixture.Create<string>() };

                    RefData
                        .GetCombinationTypeCombo(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(parameterSetsValue.Item4),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(comboNamesInSpecificLanguage);

                    comboNamesInMultilanguages.Add(standardLanguageCode, comboNamesInSpecificLanguage);
                }

                if (DoesNotContainNullOrEmptyArray(comboNamesInMultilanguages))
                {
                    expectedSyncedComboNames.Add(_parameterSets[counter], comboNamesInMultilanguages);
                }
            }

            return expectedSyncedComboNames;
        }

        private static bool DoesNotContainNullOrEmptyArray(TranslationSet comboNamesInMultilanguages)
            => comboNamesInMultilanguages.Any(pair => (pair.Value is string[] combos) && combos.Any(name => !string.IsNullOrEmpty(name)));
    }
}