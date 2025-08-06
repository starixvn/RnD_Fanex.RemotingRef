namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLotteryBetChoices = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _lotteryBetChoiceIds;

        public LotteryBetChoiceTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _lotteryBetChoiceIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLotteryBetChoices; counter++)
            {
                var lotteryBetChoiceId = Fixture.Create<string>();

                _lotteryBetChoiceIds.Add(lotteryBetChoiceId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(lotteryBetChoiceId)] = lotteryBetChoiceId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void StillAcceptBetChoiceIdInParameterSets()
        {
            // Arrange.
            var betChoiceId = Fixture.Create<string>();
            var parameterSet = new ParameterSet
            {
                [nameof(betChoiceId)] = betChoiceId
            };
            var formData = new RequestSyncFormData
            {
                ParameterSets = new List<ParameterSet> { parameterSet },
                Languages = StandardLanguageCodes
            };

            // Act.
            RefSyncController.LotteryBetChoice(formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Is(betChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void PrioritizeLotteryBetChoiceIdOverBetChoiceIdInParameterSets()
        {
            // Arrange.
            var betChoiceId = Fixture.Create<string>();
            var lotteryBetChoiceId = Fixture.Create<string>();
            var parameterSet = new ParameterSet
            {
                [nameof(betChoiceId)] = betChoiceId,
                [nameof(lotteryBetChoiceId)] = lotteryBetChoiceId
            };
            var formData = new RequestSyncFormData
            {
                ParameterSets = new List<ParameterSet> { parameterSet },
                Languages = StandardLanguageCodes
            };

            // Act.
            RefSyncController.LotteryBetChoice(formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Is(lotteryBetChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetLotteryBetChoiceOfRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachLotteryBetChoiceId()
        {
            // Act.
            RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetLotteryBetChoice(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            foreach (var lotteryBetChoiceId in _lotteryBetChoiceIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetLotteryBetChoice(
                            Arg.Is(lotteryBetChoiceId),
                            Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetChoices();

            // Act.
            var actualResult = RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetChoices(true, null);

            // Act.
            var actualResult = RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetChoices(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedLotteryBetChoices()
            => FakeSyncedLotteryBetChoices(false, null);

        private SyncedTranslationResultSet FakeSyncedLotteryBetChoices(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedLotteryBetChoiceAndTypeIds = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var lotteryBetTypeAndChoiceInMultilanguages = new TranslationSet();
                var lotteryBetChoiceId = _lotteryBetChoiceIds[counter];
                var lotteryBetTypeId = counter == specialIndex ? specialValue : Fixture.Create<string>();

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var lotteryBetChoiceNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLotteryBetChoice(
                            Arg.Is(lotteryBetChoiceId),
                            Arg.Is( standardLanguageCode))
                        .Returns((lotteryBetChoiceNameInSpecificLanguage, lotteryBetTypeId));
                    lotteryBetTypeAndChoiceInMultilanguages.Add(
                        standardLanguageCode,
                        new Tuple<string, string>(lotteryBetChoiceNameInSpecificLanguage, lotteryBetTypeId));
                }

                if (lotteryBetTypeAndChoiceInMultilanguages.Any(IsNotNullOrEmptyString))
                {
                    expectedSyncedLotteryBetChoiceAndTypeIds.Add(_parameterSets[counter], lotteryBetTypeAndChoiceInMultilanguages);
                }
            }

            return expectedSyncedLotteryBetChoiceAndTypeIds;
        }

        private static bool IsNotNullOrEmptyString(KeyValuePair<string, object> lotteryBetTypeAndChoice)
            => (lotteryBetTypeAndChoice.Value is Tuple<string, string> lotteryBetTypeAndChoicePair)
            && (!string.IsNullOrEmpty(lotteryBetTypeAndChoicePair.Item1) || !string.IsNullOrEmpty(lotteryBetTypeAndChoicePair.Item2));
    }
}