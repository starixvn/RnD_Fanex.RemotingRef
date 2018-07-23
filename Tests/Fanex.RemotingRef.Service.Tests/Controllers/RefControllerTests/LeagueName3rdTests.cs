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
            RefData.Received().GetLeagueName3rd(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
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
            var expectedLeague3rdNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var leagueNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(leagueNameInSpecificLanguage);
                expectedLeague3rdNames.Add(leagueNameInSpecificLanguage);
            }

            return expectedLeague3rdNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            Fake3rdLeagueNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            Fake3rdLeagueNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void Fake3rdLeagueNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}