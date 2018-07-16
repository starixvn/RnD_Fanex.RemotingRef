namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RefControllerBaseTests
    {
        private readonly string _matchCode;

        public BetRadarMatchCodeFormatTests()
            => _matchCode = Fixture.Create<string>();

        [Fact]
        public void CallGetBetRadarMatchCodeFormatOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetBetRadarMatchCodeFormatOfRefDataHelper()
        {
            // Act.
            RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeBetRadarMatchCodeFormats();

            // Act.
            var actualBetRadarMatchCodeFormats = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualBetRadarMatchCodeFormats.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetBetRadarMatchCodeFormatOfRefDataHelper()
        {
            // Arrange.
            var expectedBetRadarMatchCodeFormats = FakeBetRadarMatchCodeFormats();

            // Act.
            var actualBetRadarMatchCodeFormats = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetRadarMatchCodeFormats, actualBetRadarMatchCodeFormats);
        }

        private Dictionary<string, string> FakeBetRadarMatchCodeFormats()
        {
            var expectedBetRadarMatchCodeFormats = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betRadarMatchCodeFormatInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(betRadarMatchCodeFormatInSpecificLanguage);
                expectedBetRadarMatchCodeFormats.Add(standardLanguageCode, betRadarMatchCodeFormatInSpecificLanguage);
            }

            return expectedBetRadarMatchCodeFormats;
        }
    }
}