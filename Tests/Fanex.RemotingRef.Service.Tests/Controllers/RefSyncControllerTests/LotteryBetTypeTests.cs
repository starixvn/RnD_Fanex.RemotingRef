namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLotteryBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _betTypeIds;

        public LotteryBetTypeTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _betTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLotteryBetTypes; counter++)
            {
                var betTypeId = Fixture.Create<string>();

                _betTypeIds.Add(betTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    ["betTypeId"] = betTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetLotteryBetTypeOfRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            RefData.Received().GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachBetTypeId()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            foreach (var betTypeId in _betTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetLotteryBetType(Arg.Is(betTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetTypes();

            // Act.
            var actualResult = RefSyncController.LotteryBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetTypes(true, null);

            // Act.
            var actualResult = RefSyncController.LotteryBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedLotteryBetTypes(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.LotteryBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedLotteryBetTypes()
            => FakeSyncedLotteryBetTypes(false, null);

        private SyncedTranslationResultSet FakeSyncedLotteryBetTypes(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedBetTypeNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeNameInMultilanguages = new TranslationSet();
                var betTypeId = _betTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var betTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLotteryBetType(Arg.Is(betTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(betTypeNameInSpecificLanguage);
                    betTypeNameInMultilanguages.Add(standardLanguageCode, betTypeNameInSpecificLanguage);
                }

                if (ValidateTextSet(betTypeNameInMultilanguages))
                {
                    expectedSyncedBetTypeNames.Add(_parameterSets[counter], betTypeNameInMultilanguages);
                }
            }

            return expectedSyncedBetTypeNames;
        }
    }
}