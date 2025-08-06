namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RefControllerTestBase
    {
        private readonly string _betType255ResultId;

        public FastMarkets225ResultTests()
            => _betType255ResultId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetFastMarkets225ResultOfRefDataHelper()
        {
            // Act.
            RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetFastMarkets225Result(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetFastMarkets225Result(Arg.Is(_betType255ResultId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetFastMarkets225Result(Arg.Is(_betType255ResultId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarkets225Results();

            // Act.
            var actualResult = RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedFastMarkets225Results = FakeFastMarkets225Results();

            // Act.
            var actualResult = RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedFastMarkets225Results, actualResult.Values);
        }

        private List<string> FakeFastMarkets225Results()
        {
            var expectedFastMarkets225Results = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var fastMarkets225ResultInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetFastMarkets225Result(Arg.Is(_betType255ResultId), Arg.Is(standardLanguageCode))
                    .Returns(fastMarkets225ResultInSpecificLanguage);

                expectedFastMarkets225Results.Add(fastMarkets225ResultInSpecificLanguage);
            }

            return expectedFastMarkets225Results;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeFastMarkets225ResultsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeFastMarkets225ResultsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.FastMarkets225Result(_betType255ResultId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeFastMarkets225ResultsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetFastMarkets225Result(Arg.Is(_betType255ResultId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}