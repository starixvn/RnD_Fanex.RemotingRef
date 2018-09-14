namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLotteryBetChoices = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _betChoiceId;

        public LotteryBetChoiceTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _betChoiceId = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLotteryBetChoices; counter++)
            {
                var betChoiceId = Fixture.Create<string>();

                _betChoiceId.Add(betChoiceId);
                _parameterSets.Add(new ParameterSet
                {
                    ["betChoiceId"] = betChoiceId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetLotteryBetChoiceOfRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            RefData.Received().GetLotteryBetChoice(Arg.Any<string>(), Arg.Any<string>(), out _);
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
                    Arg.Any<string>(),
                    out _);
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetChoice(_formData);

            // Assert.
            foreach (var betChoiceId in _betChoiceId)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetLotteryBetChoice(
                            Arg.Is(betChoiceId),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                            out _);
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
                var lotteryBetChoiceId = _betChoiceId[counter];
                var lotteryBetTypeId = counter == specialIndex ? specialValue : Fixture.Create<string>();

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var lotteryBetChoiceNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLotteryBetChoice(
                            Arg.Is(lotteryBetChoiceId),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                            out _)
                        .Returns(parameters =>
                        {
                            parameters[2] = lotteryBetTypeId;

                            return lotteryBetChoiceNameInSpecificLanguage;
                        });
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