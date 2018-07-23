namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RefControllerTestBase
    {
        private readonly string _resultId;

        public FastMarkets225ResultTests()
            => _resultId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetFastMarkets225ResultOfRefDataHelper()
        {
            // Act.
            RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            RefData.Received().GetFastMarkets225Result(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetFastMarkets225Result(Arg.Is(_resultId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetFastMarkets225Result(Arg.Is(_resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarkets225Results();

            // Act.
            var actualResult = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedFastMarkets225Results = FakeFastMarkets225Results();

            // Act.
            var actualResult = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

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
                    .GetFastMarkets225Result(Arg.Is(_resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(fastMarkets225ResultInSpecificLanguage);
                expectedFastMarkets225Results.Add(fastMarkets225ResultInSpecificLanguage);
            }

            return expectedFastMarkets225Results;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeFastMarkets225ResultsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeFastMarkets225ResultsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeFastMarkets225ResultsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetFastMarkets225Result(Arg.Is(_resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}