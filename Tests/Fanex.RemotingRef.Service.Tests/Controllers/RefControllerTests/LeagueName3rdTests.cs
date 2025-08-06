namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RefControllerTestBase
    {
        private readonly string _league3rdId;

        public LeagueName3rdTests()
            => _league3rdId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetLeagueName3rdOfRefDataHelper()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            League3rdService
                .Received()
                .GetLeague3rdById(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            League3rdService
                .Received(StandardLanguageCodes.Length)
                .GetLeague3rdById(Arg.Is(_league3rdId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                League3rdService
                    .Received()
                    .GetLeague3rdById(Arg.Is(_league3rdId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLeague3rdNames();

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLeague3rdNames = FakeLeague3rdNames();

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLeague3rdNames, actualResult.Values);
        }

        private List<string> FakeLeague3rdNames()
        {
            var expectedLeagueNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var leagueNameInSpecificLanguage = Fixture.Create<string>();

                League3rdService
                    .GetLeague3rdById(Arg.Is(_league3rdId), Arg.Is(standardLanguageCode))
                    .Returns(leagueNameInSpecificLanguage);

                expectedLeagueNames.Add(leagueNameInSpecificLanguage);
            }

            return expectedLeagueNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            Fake3rdLeagueNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            Fake3rdLeagueNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void Fake3rdLeagueNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                League3rdService
                    .GetLeague3rdById(Arg.Is(_league3rdId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}