namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RefControllerTestBase
    {
        private readonly string _sportTypeId;

        public SportNameTests()
            => _sportTypeId = Fixture.Create<string>();

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
    }
}