namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RefControllerTestBase
    {
        private readonly string _matchCode;

        public BetRadarMatchCodeFormatTests()
            => _matchCode = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetBetRadarMatchCodeFormatOfRefDataHelper()
        {
            // Act.
            RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            RefData.Received().GetBetRadarMatchCodeFormat(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
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
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeBetRadarMatchCodeFormats();

            // Act.
            var actualResult = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedBetRadarMatchCodeFormats = FakeBetRadarMatchCodeFormats();

            // Act.
            var actualResult = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetRadarMatchCodeFormats, actualResult.Values);
        }

        private List<string> FakeBetRadarMatchCodeFormats()
        {
            var expectedBetRadarMatchCodeFormats = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betRadarMatchCodeFormatInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(betRadarMatchCodeFormatInSpecificLanguage);
                expectedBetRadarMatchCodeFormats.Add(betRadarMatchCodeFormatInSpecificLanguage);
            }

            return expectedBetRadarMatchCodeFormats;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeBetRadarMatchCodeFormatsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeBetRadarMatchCodeFormatsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.BetRadarMatchCodeFormat(_matchCode, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeBetRadarMatchCodeFormatsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetBetRadarMatchCodeFormat(Arg.Is(_matchCode), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}