namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RefSyncControllerBaseTests
    {
        private const int NumberOfTestingSportTypes = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _sportTypeIds;

        public SportNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _sportTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingSportTypes; counter++)
            {
                var sportTypeId = Fixture.Create<string>();

                _sportTypeIds.Add(sportTypeId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["sportTypeId"] = sportTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetSportNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.SportName(_formData);

            // Assert.
            RefData.Received().GetSportName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachSportTypeId()
        {
            // Act.
            RefSyncController.SportName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetSportName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.SportName(_formData);

            // Assert.
            foreach (var sportTypeId in _sportTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetSportName(Arg.Is(sportTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedSportNames();

            // Act.
            var actualResult = RefSyncController.SportName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedSportNames(true, null);

            // Act.
            var actualResult = RefSyncController.SportName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedSportNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.SportName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedSportNames(
            bool requireSpecialValue = false,
            string specialValue = null)
        {
            var expectedSyncedSportNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var sportNameInMultilanguages = new Dictionary<string, string>();
                var sportTypeId = _sportTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var sportNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetSportName(Arg.Is(sportTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(sportNameInSpecificLanguage);
                    sportNameInMultilanguages.Add(standardLanguageCode, sportNameInSpecificLanguage);
                }

                if (sportNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedSportNames.Add(_parameterSets[counter], sportNameInMultilanguages);
                }
            }

            return expectedSyncedSportNames;
        }
    }
}