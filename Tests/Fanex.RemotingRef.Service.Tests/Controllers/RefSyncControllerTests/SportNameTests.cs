namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingSportTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _sportTypeIds;

        public SportNameTests()
        {
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };
            Options.Create(fakeAppSettings);
            PrepareFormData();
        }

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _sportTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingSportTypes; counter++)
            {
                var sportTypeId = Fixture.Create<string>();

                _sportTypeIds.Add(sportTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(sportTypeId)] = sportTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };

            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetSportNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.SportName(_formData);

            // Assert.
            RefData
                .Received()
                .GetSportName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachSportTypeId()
        {
            // Act.
            RefSyncController.SportName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetSportName(Arg.Any<string>(), Arg.Any<string>());
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
                    RefData
                        .Received()
                        .GetSportName(Arg.Is(sportTypeId), Arg.Is(standardLanguageCode));
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

        private SyncedTranslationResultSet FakeSyncedSportNames()
            => FakeSyncedSportNames(false, null);

        private SyncedTranslationResultSet FakeSyncedSportNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedSportNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 0; counter < _parameterSets.Count; counter++)
            {
                var sportNameInMultilanguages = new TranslationSet();
                var sportTypeId = _sportTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var sportNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetSportName(Arg.Is(sportTypeId), Arg.Is(standardLanguageCode))
                        .Returns(sportNameInSpecificLanguage);

                    sportNameInMultilanguages.Add(standardLanguageCode, sportNameInSpecificLanguage);
                }

                // Only add parameter sets that have at least one non-empty value
                // This matches the controller's filtering logic
                if (ValidateTextSet(sportNameInMultilanguages))
                {
                    expectedSyncedSportNames.Add(_parameterSets[counter], sportNameInMultilanguages);
                }
            }

            return expectedSyncedSportNames;
        }

        // Test cases for UsingVNFunction = true
        [Fact]
        public void WhenUsingVNFunctionTrue_AlwaysCallGetSportByIdOfSportService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.SportName(_formData);

            // Assert
            SportService
                .Received()
                .GetSportById(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallSportServiceOncePerLanguageForEachSportTypeId()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.SportName(_formData);

            // Assert
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            SportService
                .Received(expectedNumberOfCalls)
                .GetSportById(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_SendCorrectArgumentsToSportService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.SportName(_formData);

            // Assert
            foreach (var sportTypeId in _sportTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    SportService
                        .Received()
                        .GetSportById(Arg.Is(sportTypeId), Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ReturnDataGotFromSportService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedResult = FakeSyncedSportNamesFromService();

            // Act
            var actualResult = RefSyncController.SportName(_formData);

            // Assert
            Assert.Equal(expectedResult.Count, actualResult.Count);
            foreach (var kvp in expectedResult)
            {
                Assert.True(actualResult.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, actualResult[kvp.Key]);
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_NullIsNotIncludedInResult()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            FakeSyncedSportNamesFromService(true, null);

            // Act
            var actualResult = RefSyncController.SportName(_formData);

            // Assert
            foreach (var kvp in actualResult)
            {
                Assert.True(kvp.Value.Count == 0);
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_EmptyStringIsNotIncludedInResult()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            FakeSyncedSportNamesFromService(true, string.Empty);

            // Act
            var actualResult = RefSyncController.SportName(_formData);

            // Assert
            foreach (var kvp in actualResult)
            {
                Assert.True(kvp.Value.Count == 0);
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_DoNotCallRefDataHelper()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.SportName(_formData);

            // Assert
            RefData
                .DidNotReceive()
                .GetSportName(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        private SyncedTranslationResultSet FakeSyncedSportNamesFromService()
        {
            var result = new SyncedTranslationResultSet();
            foreach (var parameterSet in _parameterSets)
            {
                var translations = new TranslationSet();
                var sportTypeId = parameterSet["sportTypeId"]?.ToString();
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    var value = Fixture.Create<string>();
                    translations[languageCode] = value;

                    // Mock the service call
                    SportService
                        .GetSportById(
                            Arg.Is(sportTypeId),
                            Arg.Is(languageCode))
                        .Returns(value);
                }
                result.Add(parameterSet, translations);
            }
            return result;
        }

        private void FakeSyncedSportNamesFromService(bool requireSpecialValue, string specialValue)
        {
            foreach (var parameterSet in _parameterSets)
            {
                var sportTypeId = parameterSet["sportTypeId"]?.ToString();
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    // Mock the service call
                    SportService
                        .GetSportById(
                            Arg.Is(sportTypeId),
                            Arg.Is(languageCode))
                        .Returns(specialValue);
                }
            }
        }
    }
}