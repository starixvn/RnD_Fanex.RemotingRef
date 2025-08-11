namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Controllers;
    using Fanex.RemotingRef.NetCore.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RefControllerTestBase
    {
        private readonly string _refId;
        private readonly string _resourceId;

        public ResourceDataTests()
        {
            _refId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetResourceDataOfRefDataHelper()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetResourceDataByRefIdAndResourceId(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetResourceDataByRefIdAndResourceId(Arg.Is(_refId), Arg.Is(_resourceId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeResources();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedResourceData = FakeResources();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedResourceData, actualResult.Values);
        }

        private List<string> FakeResources()
        {
            var expectedResourceData = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var resourceInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(resourceInSpecificLanguage);

                expectedResourceData.Add(resourceInSpecificLanguage);
            }

            return expectedResourceData;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeResourcesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeResourcesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0 );
        }

        private void FakeResourcesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
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
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            RefResourceService
                .Received()
                .GetRefResourceById(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallRefResourceServiceOncePerLanguage()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            RefResourceService
                .Received(StandardLanguageCodes.Length)
                .GetRefResourceById(
                    Arg.Is(_refId),
                    Arg.Is(_resourceId),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallRefResourceServiceWithCorrectArguments()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefResourceService
                    .Received()
                    .GetRefResourceById(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            FakeResourcesFromService();

            // Act
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ResultTakesDataReturnedByRefResourceServiceAsValues()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedResourceData = FakeResourcesFromService();

            // Act
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedResourceData, actualResult.Values);
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            FakeResourcesFromServiceWithSpecialValue(null);

            // Act
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            FakeResourcesFromServiceWithSpecialValue(string.Empty);

            // Act
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            Assert.True(actualResult.Count == 0);
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
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert
            RefData
                .DidNotReceive()
                .GetResourceDataByRefIdAndResourceId(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        private List<string> FakeResourcesFromService()
        {
            var expectedResourceData = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var resourceInSpecificLanguage = Fixture.Create<string>();

                RefResourceService
                    .GetRefResourceById(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(resourceInSpecificLanguage);

                expectedResourceData.Add(resourceInSpecificLanguage);
            }

            return expectedResourceData;
        }

        private void FakeResourcesFromServiceWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefResourceService
                    .GetRefResourceById(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}