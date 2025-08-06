namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsResultTests : RefControllerTestBase
    {
        private readonly string _resultId;

        public FastMarketsResultTests()
            => _resultId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetFastMarketsResultOfRefDataHelper()
        {
            // Act.
            RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetFastMarketsResult(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetFastMarketsResult(Arg.Is(_resultId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetFastMarketsResult(Arg.Is(_resultId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarketsResults();

            // Act.
            var actualResult = RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedFastMarketsResults = FakeFastMarketsResults();

            // Act.
            var actualResult = RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedFastMarketsResults, actualResult.Values);
        }

        private List<string> FakeFastMarketsResults()
        {
            var expectedFastMarketsResults = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var fastMarketsResultInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetFastMarketsResult(Arg.Is(_resultId), Arg.Is(standardLanguageCode))
                    .Returns(fastMarketsResultInSpecificLanguage);

                expectedFastMarketsResults.Add(fastMarketsResultInSpecificLanguage);
            }

            return expectedFastMarketsResults;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeFastMarketsResultsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeFastMarketsResultsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.FastMarketsResult(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeFastMarketsResultsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetFastMarketsResult(Arg.Is(_resultId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}