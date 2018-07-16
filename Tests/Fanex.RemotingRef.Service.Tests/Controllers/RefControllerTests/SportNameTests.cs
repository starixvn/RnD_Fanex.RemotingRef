namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RefControllerBaseTests
    {
        private readonly string _sportTypeId;

        public SportNameTests()
            => _sportTypeId = Fixture.Create<string>();

        [Fact]
        public void CallGetSportNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetSportName(Arg.Is(_sportTypeId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetSportNameOfRefDataHelper()
        {
            // Act.
            RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetSportName(Arg.Is(_sportTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeSportNames();

            // Act.
            var actualSportNames = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualSportNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetSportNameOfRefDataHelper()
        {
            // Arrange.
            var expectedSportNames = FakeSportNames();

            // Act.
            var actualSportNames = RefController.SportName(_sportTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedSportNames, actualSportNames);
        }

        private Dictionary<string, string> FakeSportNames()
        {
            var expectedSportNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var sportNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetSportName(Arg.Is(_sportTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(sportNameInSpecificLanguage);
                expectedSportNames.Add(standardLanguageCode, sportNameInSpecificLanguage);
            }

            return expectedSportNames;
        }
    }
}