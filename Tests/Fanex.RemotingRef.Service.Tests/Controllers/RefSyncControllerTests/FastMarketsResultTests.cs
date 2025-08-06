namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using NSubstitute;
    using Xunit;

    public class FastMarketsResultTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingResults = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _resultIds;

        public FastMarketsResultTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _resultIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingResults; counter++)
            {
                var resultId = Fixture.Create<string>();

                _resultIds.Add(resultId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(resultId)] = resultId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetFastMarketsResultOfRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarketsResult(_formData);

            // Assert.
            RefData
                .Received()
                .GetFastMarketsResult(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachResultId()
        {
            // Act.
            RefSyncController.FastMarketsResult(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetFastMarketsResult(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarketsResult(_formData);

            // Assert.
            foreach (var resultId in _resultIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetFastMarketsResult(Arg.Is(resultId), Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsResults();

            // Act.
            var actualResult = RefSyncController.FastMarketsResult(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsResults(true, null);

            // Act.
            var actualResult = RefSyncController.FastMarketsResult(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarketsResults(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.FastMarketsResult(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedFastMarketsResults()
            => FakeSyncedFastMarketsResults(false, null);

        private SyncedTranslationResultSet FakeSyncedFastMarketsResults(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedFastMarketsResults = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var fastMarketsResultInMultilanguages = new TranslationSet();
                var resultId = _resultIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var fastMarketsResultInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetFastMarketsResult(Arg.Is(resultId), Arg.Is(standardLanguageCode))
                        .Returns(fastMarketsResultInSpecificLanguage);

                    fastMarketsResultInMultilanguages.Add(standardLanguageCode, fastMarketsResultInSpecificLanguage);
                }

                if (ValidateTextSet(fastMarketsResultInMultilanguages))
                {
                    expectedSyncedFastMarketsResults.Add(_parameterSets[counter], fastMarketsResultInMultilanguages);
                }
            }

            return expectedSyncedFastMarketsResults;
        }
    }
}