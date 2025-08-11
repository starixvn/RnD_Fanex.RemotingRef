namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Controllers;
    using Fanex.RemotingRef.NetCore.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefControllerTestBase
    {
        private readonly string _betTypeId;
        private readonly string _resourceId;
        private readonly long? _refNo;
        
        public BetTypeNameTests()
        {
            _betTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

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
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetBetTypeName(
                    Arg.Is(_betTypeId),
                    Arg.Is(_resourceId),
                    Arg.Is(_refNo),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeBetTypeNames();

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedBetTypeNames = FakeBetTypeNames();

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetTypeNames, actualResult.Values);
        }

        private List<string> FakeBetTypeNames()
        {
            var expectedBetTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(betTypeNameInSpecificLanguage);

                expectedBetTypeNames.Add(betTypeNameInSpecificLanguage);
            }

            return expectedBetTypeNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeBetTypeNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeBetTypeNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeBetTypeNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
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
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

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
        public void WhenUsingVNFunctionTrue_CallBettypeServiceOncePerLanguage()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert
            BettypeService
                .Received(StandardLanguageCodes.Length)
                .GetBettypeById(
                    Arg.Is(_betTypeId),
                    Arg.Is(_resourceId),
                    Arg.Is(_refNo),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallBettypeServiceWithCorrectArguments()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                BettypeService
                    .Received()
                    .GetBettypeById(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
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
            FakeBetTypeNamesFromService();

            // Act
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ResultTakesDataReturnedByBettypeServiceAsValues()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedBetTypeNames = FakeBetTypeNamesFromService();

            // Act
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedBetTypeNames, actualResult.Values);
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
            FakeBetTypeNamesFromServiceWithSpecialValue(null);

            // Act
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

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
            FakeBetTypeNamesFromServiceWithSpecialValue(string.Empty);

            // Act
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

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
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert
            RefData
                .DidNotReceive()
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        private List<string> FakeBetTypeNamesFromService()
        {
            var expectedBetTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betTypeNameInSpecificLanguage = Fixture.Create<string>();

                BettypeService
                    .GetBettypeById(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(betTypeNameInSpecificLanguage);

                expectedBetTypeNames.Add(betTypeNameInSpecificLanguage);
            }

            return expectedBetTypeNames;
        }

        private void FakeBetTypeNamesFromServiceWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                BettypeService
                    .GetBettypeById(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}