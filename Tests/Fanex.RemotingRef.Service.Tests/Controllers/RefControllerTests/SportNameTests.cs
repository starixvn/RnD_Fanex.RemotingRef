namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Controllers;
    using Fanex.RemotingRef.NetCore.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RefControllerTestBase
    {
        private readonly string _sportTypeId;

        public SportNameTests()
        {
            _sportTypeId = Fixture.Create<string>();
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };
            Options.Create(fakeAppSettings);
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void AlwaysCallGetSportNameOfRefDataHelper()
        {
            // Act.
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetSportName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetSportName(Arg.Is(_sportTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetSportName(Arg.Is(_sportTypeId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeSportNames();

            // Act.
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedSportNames = FakeSportNames();

            // Act.
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedSportNames, actualResult.Values);
        }

        private List<string> FakeSportNames()
        {
            var expectedSportNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var sportNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetSportName(Arg.Is(_sportTypeId), Arg.Is(standardLanguageCode))
                    .Returns(sportNameInSpecificLanguage);

                expectedSportNames.Add(sportNameInSpecificLanguage);
            }

            return expectedSportNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeSportNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeSportNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeSportNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetSportName(Arg.Is(_sportTypeId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
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
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            SportService
                .Received()
                .GetSportById(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallSportServiceOncePerLanguage()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            SportService
                .Received(StandardLanguageCodes.Length)
                .GetSportById(
                    Arg.Is(_sportTypeId),
                    Arg.Any<string>());
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_CallSportServiceWithCorrectArguments()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

            // Act
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                SportService
                    .Received()
                    .GetSportById(
                        Arg.Is(_sportTypeId),
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
            FakeSportNamesFromService();

            // Act
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void WhenUsingVNFunctionTrue_ResultTakesDataReturnedBySportServiceAsValues()
        {
            // Arrange
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = true
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            var expectedSportNames = FakeSportNamesFromService();

            // Act
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedSportNames, actualResult.Values);
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
            FakeSportNamesFromServiceWithSpecialValue(null);

            // Act
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

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
            FakeSportNamesFromServiceWithSpecialValue(string.Empty);

            // Act
            var actualResult = RefController.SportName(_sportTypeId, StandardLanguageCodes);

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
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert
            RefData
                .DidNotReceive()
                .GetSportName(
                    Arg.Any<string>(),
                    Arg.Any<string>());
        }

        private List<string> FakeSportNamesFromService()
        {
            var expectedSportNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var sportNameInSpecificLanguage = Fixture.Create<string>();

                SportService
                    .GetSportById(
                        Arg.Is(_sportTypeId),
                        Arg.Is(standardLanguageCode))
                    .Returns(sportNameInSpecificLanguage);

                expectedSportNames.Add(sportNameInSpecificLanguage);
            }

            return expectedSportNames;
        }

        private void FakeSportNamesFromServiceWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                SportService
                    .GetSportById(
                        Arg.Is(_sportTypeId),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}