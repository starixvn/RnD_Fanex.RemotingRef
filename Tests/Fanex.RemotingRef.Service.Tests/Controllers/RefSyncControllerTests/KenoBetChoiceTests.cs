namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class KenoBetChoiceTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingKenoBetChoices = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _kenoBetChoiceIds;

        public KenoBetChoiceTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _kenoBetChoiceIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingKenoBetChoices; counter++)
            {
                var kenoBetChoiceId = Fixture.Create<string>();

                _kenoBetChoiceIds.Add(kenoBetChoiceId);
                _parameterSets.Add(new ParameterSet
                {
                    ["kenoBetChoiceId"] = kenoBetChoiceId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetKenoBetChoiceOfRefDataHelper()
        {
            // Act.
            RefSyncController.KenoBetChoice(_formData);

            // Assert.
            RefData.Received().GetKenoBetChoice(Arg.Any<string>(), Arg.Any<string>(), out _);
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachKenoBetChoiceId()
        {
            // Act.
            RefSyncController.KenoBetChoice(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetKenoBetChoice(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    out _);
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.KenoBetChoice(_formData);

            // Assert.
            foreach (var kenoBetChoiceId in _kenoBetChoiceIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetKenoBetChoice(
                            Arg.Is(kenoBetChoiceId),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                            out _);
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetChoices();

            // Act.
            var actualResult = RefSyncController.KenoBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetChoices(true, null);

            // Act.
            var actualResult = RefSyncController.KenoBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetChoices(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.KenoBetChoice(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedKenoBetChoices()
            => FakeSyncedKenoBetChoices(false, null);

        private SyncedTranslationResultSet FakeSyncedKenoBetChoices(
            bool requireSpecialValue,
            string specialValue)
        {
            var expectedSyncedKenoBetChoiceAndTypeIds = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var kenoBetTypeAndChoiceInMultilanguages = new TranslationSet();
                var kenoBetChoiceId = _kenoBetChoiceIds[counter];
                var kenoBetTypeId = counter == specialIndex ? specialValue : Fixture.Create<string>();

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var kenoBetChoiceNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetKenoBetChoice(
                            Arg.Is(kenoBetChoiceId),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                            out _)
                        .Returns(parameters =>
                        {
                            parameters[2] = kenoBetTypeId;

                            return kenoBetChoiceNameInSpecificLanguage;
                        });
                    kenoBetTypeAndChoiceInMultilanguages.Add(
                        standardLanguageCode,
                        new Tuple<string, string>(kenoBetChoiceNameInSpecificLanguage, kenoBetTypeId));
                }

                if (kenoBetTypeAndChoiceInMultilanguages.Any(IsNotNullOrEmptyString))
                {
                    expectedSyncedKenoBetChoiceAndTypeIds.Add(_parameterSets[counter], kenoBetTypeAndChoiceInMultilanguages);
                }
            }

            return expectedSyncedKenoBetChoiceAndTypeIds;
        }

        private static bool IsNotNullOrEmptyString(KeyValuePair<string, object> kenoBetTypeAndChoice)
            => (kenoBetTypeAndChoice.Value is Tuple<string, string> kenoBetTypeAndChoicePair)
            && (!string.IsNullOrEmpty(kenoBetTypeAndChoicePair.Item1) || !string.IsNullOrEmpty(kenoBetTypeAndChoicePair.Item2));
    }
}