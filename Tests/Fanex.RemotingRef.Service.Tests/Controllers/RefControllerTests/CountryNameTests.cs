namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RefControllerBaseTests
    {
        private readonly string _countryCode;

        public CountryNameTests()
            => _countryCode = Fixture.Create<string>();

        [Fact]
        public void CallGetCountryNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetCountryName(Arg.Is(_countryCode), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetCountryNameOfRefDataHelper()
        {
            // Act.
            RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetCountryName(Arg.Is(_countryCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCountryNames();

            // Act.
            var actualCountryNames = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualCountryNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetCountryNameOfRefDataHelper()
        {
            // Arrange.
            var expectedCountryNames = FakeCountryNames();

            // Act.
            var actualCountryNames = RefController.CountryName(_countryCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCountryNames, actualCountryNames);
        }

        private Dictionary<string, string> FakeCountryNames()
        {
            var expectedCountryNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var countryNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetCountryName(Arg.Is(_countryCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(countryNameInSpecificLanguage);
                expectedCountryNames.Add(standardLanguageCode, countryNameInSpecificLanguage);
            }

            return expectedCountryNames;
        }
    }
}