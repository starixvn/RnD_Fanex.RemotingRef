namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingCountries = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _countryCodes;

        public CountryNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _countryCodes = new List<string>();

            for (var counter = 0; counter < NumberOfTestingCountries; counter++)
            {
                var countryCode = Fixture.Create<string>();

                _countryCodes.Add(countryCode);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["countryCode"] = countryCode
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetCountryNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.CountryName(_formData);

            // Assert.
            RefData.Received().GetCountryName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachCountryCode()
        {
            // Act.
            RefSyncController.CountryName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetCountryName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.CountryName(_formData);

            // Assert.
            foreach (var countryCode in _countryCodes)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetCountryName(Arg.Is(countryCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedCountryNames();

            // Act.
            var actualResult = RefSyncController.CountryName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCountryNames(true, null);

            // Act.
            var actualResult = RefSyncController.CountryName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedCountryNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.CountryName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedCountryNames()
            => FakeSyncedCountryNames(false, null);

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedCountryNames(
            bool requireSpecialValue,
            string specialValue)
        {
            var expectedSyncedCountryNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var countryNameInMultilanguages = new Dictionary<string, string>();
                var countryCode = _countryCodes[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var countryNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetCountryName(Arg.Is(countryCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(countryNameInSpecificLanguage);
                    countryNameInMultilanguages.Add(standardLanguageCode, countryNameInSpecificLanguage);
                }

                if (countryNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedCountryNames.Add(_parameterSets[counter], countryNameInMultilanguages);
                }
            }

            return expectedSyncedCountryNames;
        }
    }
}