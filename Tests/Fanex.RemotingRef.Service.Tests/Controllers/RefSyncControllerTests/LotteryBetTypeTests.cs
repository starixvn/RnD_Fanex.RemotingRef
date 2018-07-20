namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RefSyncControllerBaseTests
    {
        private const int NumberOfTestingLotteryBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _betTypeIds;

        public LotteryBetTypeTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _betTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingLotteryBetTypes; counter++)
            {
                var betTypeId = Fixture.Create<string>();

                _betTypeIds.Add(betTypeId);
                _parameterSets.Add(new Dictionary<string, object>
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

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedLotteryBetTypes(
            bool requireSpecialValue = false,
            string specialValue = null)
        {
            var expectedSyncedBetTypeNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeNameInMultilanguages = new Dictionary<string, string>();
                var betTypeId = _betTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var betTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetLotteryBetType(Arg.Is(betTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(betTypeNameInSpecificLanguage);
                    betTypeNameInMultilanguages.Add(standardLanguageCode, betTypeNameInSpecificLanguage);
                }

                if (betTypeNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedBetTypeNames.Add(_parameterSets[counter], betTypeNameInMultilanguages);
                }
            }

            return expectedSyncedBetTypeNames;
        }
    }
}