namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RefControllerTestBase
    {
        private readonly string _countryCode;

        public CountryNameTests()
            => _countryCode = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetCountryNameOfRefDataHelper()
        {
            // Act.
            RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetCountryName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetCountryName(Arg.Is(_countryCode), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetCountryName(Arg.Is(_countryCode), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCountryNames();

            // Act.
            var actualResult = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedCountryNames = FakeCountryNames();

            // Act.
            var actualResult = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCountryNames, actualResult.Values);
        }

        private List<string> FakeCountryNames()
        {
            var expectedCountryNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var countryNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetCountryName(Arg.Is(_countryCode), Arg.Is(standardLanguageCode))
                    .Returns(countryNameInSpecificLanguage);

                expectedCountryNames.Add(countryNameInSpecificLanguage);
            }

            return expectedCountryNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeCountryNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeCountryNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeCountryNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetCountryName(Arg.Is(_countryCode), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}