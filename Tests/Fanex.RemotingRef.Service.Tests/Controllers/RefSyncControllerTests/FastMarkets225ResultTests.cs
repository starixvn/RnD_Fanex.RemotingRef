namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingResults = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _resultIds;

        public FastMarkets225ResultTests()
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
                    ["resultId"] = resultId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetFastMarkets225ResultOfRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            RefData.Received().GetFastMarkets225Result(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachResultId()
        {
            // Act.
            RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetFastMarkets225Result(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            foreach (var resultId in _resultIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetFastMarkets225Result(Arg.Is(resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarkets225Results();

            // Act.
            var actualResult = RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarkets225Results(true, null);

            // Act.
            var actualResult = RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedFastMarkets225Results(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.FastMarkets225Result(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedFastMarkets225Results()
            => FakeSyncedFastMarkets225Results(false, null);

        private SyncedTranslationResultSet FakeSyncedFastMarkets225Results(
            bool requireSpecialValue,
            string specialValue)
        {
            var expectedSyncedFastMarkets225Results = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var fastMarkets225ResultInMultilanguages = new TranslationSet();
                var resultId = _resultIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var fastMarkets225ResultInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetFastMarkets225Result(Arg.Is(resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(fastMarkets225ResultInSpecificLanguage);
                    fastMarkets225ResultInMultilanguages.Add(standardLanguageCode, fastMarkets225ResultInSpecificLanguage);
                }

                if (ValidateTextSet(fastMarkets225ResultInMultilanguages))
                {
                    expectedSyncedFastMarkets225Results.Add(_parameterSets[counter], fastMarkets225ResultInMultilanguages);
                }
            }

            return expectedSyncedFastMarkets225Results;
        }
    }
}