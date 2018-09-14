namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingRouletteBetItems = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _itemIds;

        public RouletteBetItemTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _itemIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingRouletteBetItems; counter++)
            {
                var itemId = Fixture.Create<string>();

                _itemIds.Add(itemId);
                _parameterSets.Add(new ParameterSet
                {
                    ["itemId"] = itemId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetRouletteBetItemOfRefDataHelper()
        {
            // Act.
            RefSyncController.RouletteBetItem(_formData);

            // Assert.
            RefData.Received().GetRouletteBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachItemId()
        {
            // Act.
            RefSyncController.RouletteBetItem(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetRouletteBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.RouletteBetItem(_formData);

            // Assert.
            foreach (var itemId in _itemIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetRouletteBetItem(Arg.Is(itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedRouletteBetItems();

            // Act.
            var actualResult = RefSyncController.RouletteBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedRouletteBetItems(true, null);

            // Act.
            var actualResult = RefSyncController.RouletteBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedRouletteBetItems(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.RouletteBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedRouletteBetItems()
            => FakeSyncedRouletteBetItems(false, null);

        private SyncedTranslationResultSet FakeSyncedRouletteBetItems(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedRouletteBetItems = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betItemInMultilanguages = new TranslationSet();
                var itemId = _itemIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var betItemInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetRouletteBetItem(Arg.Is(itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(betItemInSpecificLanguage);
                    betItemInMultilanguages.Add(standardLanguageCode, betItemInSpecificLanguage);
                }

                if (ValidateTextSet(betItemInMultilanguages))
                {
                    expectedSyncedRouletteBetItems.Add(_parameterSets[counter], betItemInMultilanguages);
                }
            }

            return expectedSyncedRouletteBetItems;
        }
    }
}