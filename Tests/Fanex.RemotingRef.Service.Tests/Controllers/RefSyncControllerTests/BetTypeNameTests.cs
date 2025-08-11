namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Controllers;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefSyncControllerTestBase
    {
        private readonly RequestSyncFormData _formData;
        private readonly List<ParameterSet> _parameterSets;
        private readonly List<Tuple<string, string, long?>> _parameterSetsValues;

        public BetTypeNameTests()
        {
            _parameterSets = new List<ParameterSet>();
            _parameterSetsValues = new List<Tuple<string, string, long?>>();
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
            PrepareFormData();
        }

        private void PrepareFormData()
        {
            AddParameterSet(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<long?>());
            AddParameterSet(
                Fixture.Create<string>(),
                null,
                Fixture.Create<long?>());
            AddParameterSet(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                null);
            AddParameterSet(
                Fixture.Create<string>(),
                null,
                null);
        }

        private void AddParameterSet(string betTypeId, string resourceId, long? refNo)
        {
            _parameterSetsValues.Add(new Tuple<string, string, long?>(betTypeId, resourceId, refNo));
            _parameterSets.Add(new ParameterSet
            {
                [nameof(betTypeId)] = betTypeId,
                [nameof(resourceId)] = resourceId,
                [nameof(refNo)] = refNo
            });
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

            // Assert.
            RefData
                .Received()
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachParameterSet()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

            // Assert.
            foreach (var parameterSetsValue in _parameterSetsValues)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetBetTypeName(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetTypeNames();

            // Act.
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange
            var expectedResult = FakeSyncedBetTypeNames(true, null);

            // Act
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert
            Assert.Equal(expectedResult.Count, actualResult.Count);
            // When all values are null, the result should be empty
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange
            var expectedResult = FakeSyncedBetTypeNames(true, string.Empty);

            // Act
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert
            Assert.Equal(expectedResult.Count, actualResult.Count);
            // When all values are empty strings, the result should be empty
            Assert.True(actualResult.Count == 0);
        }

        private SyncedTranslationResultSet FakeSyncedBetTypeNames()
            => FakeSyncedBetTypeNames(false, null);

        private SyncedTranslationResultSet FakeSyncedBetTypeNames(bool requireSpecialValue, string specialValue)
        {
            var result = new SyncedTranslationResultSet();
            foreach (var parameterSet in _parameterSets)
            {
                var translations = new TranslationSet();
                var parameterSetValues = _parameterSetsValues[_parameterSets.IndexOf(parameterSet)];
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    var value = requireSpecialValue ? specialValue : Fixture.Create<string>();
                    translations[languageCode] = value;

                    // Mock the RefData call
                    RefData
                        .GetBetTypeName(
                            Arg.Is(parameterSetValues.Item1), // betTypeId
                            Arg.Is(parameterSetValues.Item2), // resourceId
                            Arg.Is(parameterSetValues.Item3), // refNo
                            Arg.Is(languageCode))
                        .Returns(value);
                }

                // Only add parameter sets that have at least one non-empty value
                // This matches the controller's filtering logic
                if (!requireSpecialValue || !string.IsNullOrEmpty(specialValue))
                {
                    result.Add(parameterSet, translations);
                }
            }
            return result;
        }

        // Test cases for UsingVNFunction = true
        [Fact]
        public void WhenUsingVNFunctionTrue_AlwaysCallGetBettypeByIdOfBettypeService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.BetTypeName(_formData);

            // Assert
            BettypeService
                .Received()
                .GetBettypeById(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallBettypeServiceOncePerLanguageForEachParameterSet()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.BetTypeName(_formData);

            // Assert
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            BettypeService
                .Received(expectedNumberOfCalls)
                .GetBettypeById(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_SendCorrectArgumentsToBettypeService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefSyncController.BetTypeName(_formData);

            // Assert
            for (int i = 0; i < _parameterSets.Count; i++)
            {
                var parameterSet = _parameterSets[i];
                var parameterSetValues = _parameterSetsValues[i];

                foreach (var languageCode in StandardLanguageCodes)
                {
                    BettypeService
                        .Received()
                        .GetBettypeById(
                            Arg.Is(parameterSetValues.Item1), // betTypeId
                            Arg.Is(parameterSetValues.Item2), // resourceId
                            Arg.Is(parameterSetValues.Item3), // refNo
                            Arg.Is(languageCode));
                }
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ReturnDataGotFromBettypeService()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedResult = FakeSyncedBetTypeNamesFromService();

            // Act
            var actualResult = RefSyncController.BetTypeName(_formData);

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
            FakeSyncedBetTypeNamesFromService(true, null);

            // Act
            var actualResult = RefSyncController.BetTypeName(_formData);

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
            FakeSyncedBetTypeNamesFromService(true, string.Empty);

            // Act
            var actualResult = RefSyncController.BetTypeName(_formData);

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
            RefSyncController.BetTypeName(_formData);

            // Assert
            RefData
                .DidNotReceive()
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        private SyncedTranslationResultSet FakeSyncedBetTypeNamesFromService()
        {
            var result = new SyncedTranslationResultSet();
            foreach (var parameterSet in _parameterSets)
            {
                var translations = new TranslationSet();
                
                foreach (var languageCode in StandardLanguageCodes)
                {
                    var value = Fixture.Create<string>();
                    translations[languageCode] = value;

                    // Mock the service call
                    BettypeService
                        .GetBettypeById(
                            Arg.Is(parameterSet["betTypeId"]?.ToString()),
                            Arg.Is(parameterSet["resourceId"]?.ToString()),
                            Arg.Is(parameterSet["refNo"] != null ? Convert.ToInt64(parameterSet["refNo"]) : (long?)null),
                            Arg.Is(languageCode))
                        .Returns(value);
                }
                result.Add(parameterSet, translations);
            }
            return result;
        }

        private void FakeSyncedBetTypeNamesFromService(bool requireSpecialValue, string specialValue)
        {
            foreach (var parameterSet in _parameterSets)
            {
                foreach (var languageCode in StandardLanguageCodes)
                {
                    // Mock the service call
                    BettypeService
                        .GetBettypeById(
                            Arg.Is(parameterSet["betTypeId"]?.ToString()),
                            Arg.Is(parameterSet["resourceId"]?.ToString()),
                            Arg.Is(parameterSet["refNo"] != null ? Convert.ToInt64(parameterSet["refNo"]) : (long?)null),
                            Arg.Is(languageCode))
                        .Returns(specialValue);
                }
            }
        }
    }
}