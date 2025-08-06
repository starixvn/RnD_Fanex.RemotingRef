namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingCasinoBetItems = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _itemIds;

        public CasinoBetItemTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _itemIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingCasinoBetItems; counter++)
            {
                var itemId = Fixture.Create<string>();

                _itemIds.Add(itemId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(itemId)] = itemId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetCasinoBetItemOfRefDataHelper()
        {
            // Act.
            RefSyncController.CasinoBetItem(_formData);

            // Assert.
            RefData
                .Received()
                .GetCasinoBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachItemId()
        {
            // Act.
            RefSyncController.CasinoBetItem(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetCasinoBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.CasinoBetItem(_formData);

            // Assert.
            foreach (var itemId in _itemIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetCasinoBetItem(Arg.Is(itemId), Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedCasinoBetItems();

            // Act.
            var actualResult = RefSyncController.CasinoBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCasinoBetItems(true, null);

            // Act.
            var actualResult = RefSyncController.CasinoBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCasinoBetItems(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.CasinoBetItem(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedCasinoBetItems()
            => FakeSyncedCasinoBetItems(false, null);

        private SyncedTranslationResultSet FakeSyncedCasinoBetItems(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedCasinoBetItems = new SyncedTranslationResultSet();
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
                        .GetCasinoBetItem(Arg.Is(itemId), Arg.Is(standardLanguageCode))
                        .Returns(betItemInSpecificLanguage);

                    betItemInMultilanguages.Add(standardLanguageCode, betItemInSpecificLanguage);
                }

                if (ValidateTextSet(betItemInMultilanguages))
                {
                    expectedSyncedCasinoBetItems.Add(_parameterSets[counter], betItemInMultilanguages);
                }
            }

            return expectedSyncedCasinoBetItems;
        }
    }
}