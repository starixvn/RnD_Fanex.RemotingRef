namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RefControllerBaseTests
    {
        private readonly string _league3rdId;

        public LeagueName3rdTests()
            => _league3rdId = Fixture.Create<string>();

        [Fact]
        public void CallGetLeagueName3rdOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetLeagueName3rdOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLeague3rdNames();

            // Act.
            var actualLeague3rdNames = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualLeague3rdNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetLeagueName3rdOfRefDataHelper()
        {
            // Arrange.
            var expectedLeague3rdNames = FakeLeague3rdNames();

            // Act.
            var actualLeague3rdNames = RefController.LeagueName3rd(_league3rdId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLeague3rdNames, actualLeague3rdNames);
        }

        private Dictionary<string, string> FakeLeague3rdNames()
        {
            var expectedLeague3rdNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var leagueNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLeagueName3rd(Arg.Is(_league3rdId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(leagueNameInSpecificLanguage);
                expectedLeague3rdNames.Add(standardLanguageCode, leagueNameInSpecificLanguage);
            }

            return expectedLeague3rdNames;
        }
    }
}