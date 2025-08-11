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

    public class LotteryBetTypeTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingLotteryBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _lotteryBetTypeIds;

        public LotteryBetTypeTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _lotteryBetTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLotteryBetTypes; counter++)
            {
                var lotteryBetTypeId = Fixture.Create<string>();

                _lotteryBetTypeIds.Add(lotteryBetTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(lotteryBetTypeId)] = lotteryBetTypeId
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
        public void StillAcceptBetTypeIdInParameterSets()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var parameterSet = new ParameterSet
            {
                [nameof(betTypeId)] = betTypeId
            };
            var formData = new RequestSyncFormData
            {
                ParameterSets = new List<ParameterSet> { parameterSet },
                Languages = StandardLanguageCodes
            };

            // Act.
            RefSyncController.LotteryBetType(formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Is(betTypeId), Arg.Any<string>());
        }

        [Fact]
        public void PrioritizeLotteryBetTypeIdOverBetTypeIdInParameterSets()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var lotteryBetTypeId = Fixture.Create<string>();
            var parameterSet = new ParameterSet
            {
                [nameof(betTypeId)] = betTypeId,
                [nameof(lotteryBetTypeId)] = lotteryBetTypeId
            };
            var formData = new RequestSyncFormData
            {
                ParameterSets = new List<ParameterSet> { parameterSet },
                Languages = StandardLanguageCodes
            };

            // Act.
            RefSyncController.LotteryBetType(formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Is(lotteryBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetLotteryBetTypeOfRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachBetTypeId()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.LotteryBetType(_formData);

            // Assert.
            foreach (var lotteryBetTypeId in _lotteryBetTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetLotteryBetType(Arg.Is(lotteryBetTypeId), Arg.Is(standardLanguageCode));
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
            var expectedSyncedLotteryBetTypeNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var lotteryBetTypeNameInMultilanguages = new TranslationSet();
                var lotteryBetTypeId = _lotteryBetTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var lotteryBetTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLotteryBetType(Arg.Is(lotteryBetTypeId), Arg.Is(standardLanguageCode))
                        .Returns(lotteryBetTypeNameInSpecificLanguage);

                    lotteryBetTypeNameInMultilanguages.Add(standardLanguageCode, lotteryBetTypeNameInSpecificLanguage);
                }

                if (ValidateTextSet(lotteryBetTypeNameInMultilanguages))
                {
                    expectedSyncedLotteryBetTypeNames.Add(_parameterSets[counter], lotteryBetTypeNameInMultilanguages);
                }
            }

            return expectedSyncedLotteryBetTypeNames;
        }
    }
}