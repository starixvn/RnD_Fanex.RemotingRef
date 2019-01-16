namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class BetBuilderSelectionNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingSelections = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<int> _selectionIds;

        public BetBuilderSelectionNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _selectionIds = new List<int>();

            for (var counter = 0; counter < NumberOfTestingSelections; counter++)
            {
                var selectionId = Fixture.Create<int>();

                _selectionIds.Add(selectionId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(selectionId)] = selectionId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetBetBuilderSelectionNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            RefData
                .Received()
                .GetBetBuilderSelectionName(Arg.Any<int>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachSelectionId()
        {
            // Act.
            RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetBetBuilderSelectionName(Arg.Any<int>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            foreach (var selectionId in _selectionIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetBetBuilderSelectionName(Arg.Is(selectionId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetBuilderSelectionNames();

            // Act.
            var actualResult = RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetBuilderSelectionNames(true, null);

            // Act.
            var actualResult = RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetBuilderSelectionNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.BetBuilderSelectionName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedBetBuilderSelectionNames()
            => FakeSyncedBetBuilderSelectionNames(false, null);

        private SyncedTranslationResultSet FakeSyncedBetBuilderSelectionNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedSelectionNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var selectionNameInMultilanguages = new TranslationSet();
                var selectionId = _selectionIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var selectionNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetBetBuilderSelectionName(Arg.Is(selectionId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(selectionNameInSpecificLanguage);

                    selectionNameInMultilanguages.Add(standardLanguageCode, selectionNameInSpecificLanguage);
                }

                if (ValidateTextSet(selectionNameInMultilanguages))
                {
                    expectedSyncedSelectionNames.Add(_parameterSets[counter], selectionNameInMultilanguages);
                }
            }

            return expectedSyncedSelectionNames;
        }
    }
}