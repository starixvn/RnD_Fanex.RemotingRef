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

    public class ResourceDataTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingResources = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<Tuple<string, string>> _refIdAndResourceIdPairs;

        public ResourceDataTests()
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
            _refIdAndResourceIdPairs = new List<Tuple<string, string>>();

            for (var counter = 0; counter < NumberOfTestingResources; counter++)
            {
                var refId = Fixture.Create<string>();
                var resourceId = Fixture.Create<string>();

                _refIdAndResourceIdPairs.Add(new Tuple<string, string>(refId, resourceId));
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(refId)] = refId,
                    [nameof(resourceId)] = resourceId
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
        public void AlwaysGetResourceDataByRefIdAndResourceIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            RefData
                .Received()
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachPairOfRefIdAndResourceId()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            foreach (var pair in _refIdAndResourceIdPairs)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetResourceDataByRefIdAndResourceId(
                            Arg.Is(pair.Item1),
                            Arg.Is(pair.Item2),
                            Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData();

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData(true, null);

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedResourceData()
            => FakeSyncedResourceData(false, null);

        private SyncedTranslationResultSet FakeSyncedResourceData(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedResourceData = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 0; counter < _parameterSets.Count; counter++)
            {
                var resourceDataInMultilanguages = new TranslationSet();
                var refIdAndResourceIdPair = _refIdAndResourceIdPairs[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var resourceDataInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetResourceDataByRefIdAndResourceId(
                            Arg.Is(refIdAndResourceIdPair.Item1),
                            Arg.Is(refIdAndResourceIdPair.Item2),
                            Arg.Is(standardLanguageCode))
                        .Returns(resourceDataInSpecificLanguage);

                    resourceDataInMultilanguages.Add(standardLanguageCode, resourceDataInSpecificLanguage);
                }

                // Only add parameter sets that have at least one non-empty value
                // This matches the controller's filtering logic
                if (ValidateTextSet(resourceDataInMultilanguages))
                {
                    expectedSyncedResourceData.Add(_parameterSets[counter], resourceDataInMultilanguages);
                }
            }

            return expectedSyncedResourceData;
        }

        // Test cases for UsingVNFunction = true
        [Fact]
        public void WhenUsingVNFunctionTrue_AlwaysCallGetRefResourceByIdOfRefResourceService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.ResourceData(_formData);

            // Assert
            RefResourceService
                .Received()
                .GetRefResourceById(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallRefResourceServiceOncePerLanguageForEachPairOfRefIdAndResourceId()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.ResourceData(_formData);

            // Assert
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefResourceService
                .Received(expectedNumberOfCalls)
                .GetRefResourceById(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_SendCorrectArgumentsToRefResourceService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.ResourceData(_formData);

            // Assert
            foreach (var refIdAndResourceIdPair in _refIdAndResourceIdPairs)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefResourceService
                        .Received()
                        .GetRefResourceById(
                            Arg.Is(refIdAndResourceIdPair.Item1), // refId
                            Arg.Is(refIdAndResourceIdPair.Item2), // resourceId
                            Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ReturnDataGotFromRefResourceService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedResult = FakeSyncedResourceDataFromService();

            // Act
            var actualResult = RefSyncController.ResourceData(_formData);

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
            FakeSyncedResourceDataFromService(true, null);

            // Act
            var actualResult = RefSyncController.ResourceData(_formData);

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
            FakeSyncedResourceDataFromService(true, string.Empty);

            // Act
            var actualResult = RefSyncController.ResourceData(_formData);

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
            RefSyncController.ResourceData(_formData);

            // Assert
            RefData
                .DidNotReceive()
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        private SyncedTranslationResultSet FakeSyncedResourceDataFromService()
        {
            var result = new SyncedTranslationResultSet();
            foreach (var parameterSet in _parameterSets)
            {
                var translations = new TranslationSet();
                var refId = parameterSet["refId"]?.ToString();
                var resourceId = parameterSet["resourceId"]?.ToString();
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    var value = Fixture.Create<string>();
                    translations[languageCode] = value;

                    // Mock the service call
                    RefResourceService
                        .GetRefResourceById(
                            Arg.Is(refId),
                            Arg.Is(resourceId),
                            Arg.Is(languageCode))
                        .Returns(value);
                }
                result.Add(parameterSet, translations);
            }
            return result;
        }

        private void FakeSyncedResourceDataFromService(bool requireSpecialValue, string specialValue)
        {
            foreach (var parameterSet in _parameterSets)
            {
                var refId = parameterSet["refId"]?.ToString();
                var resourceId = parameterSet["resourceId"]?.ToString();
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    // Mock the service call
                    RefResourceService
                        .GetRefResourceById(
                            Arg.Is(refId),
                            Arg.Is(resourceId),
                            Arg.Is(languageCode))
                        .Returns(specialValue);
                }
            }
        }
    }
}