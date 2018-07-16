namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsChoiceTests : RefControllerBaseTests
    {
        private readonly string _betTypeId;
        private readonly string _betTeam;
        private readonly int _handicap1;
        private readonly int _handicap2;

        public FastMarketsChoiceTests()
        {
            _betTypeId = Fixture.Create<string>();
            _betTeam = Fixture.Create<string>();
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();
        }

        [Fact]
        public void CallGetFastMarketsChoiceOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetFastMarketsChoice(
                    Arg.Is(_betTypeId),
                    Arg.Is(_betTeam),
                    Arg.Is(_handicap1),
                    Arg.Is(_handicap2),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetFastMarketsChoiceOfRefDataHelper()
        {
            // Act.
            RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetFastMarketsChoice(
                        Arg.Is(_betTypeId),
                        Arg.Is(_betTeam),
                        Arg.Is(_handicap1),
                        Arg.Is(_handicap2),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarketsChoices();

            // Act.
            var actualFastMarketsChoices = RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualFastMarketsChoices.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetFastMarketsChoiceOfRefDataHelper()
        {
            // Arrange.
            var expectedFastMarketsChoices = FakeFastMarketsChoices();

            // Act.
            var actualFastMarketsChoices = RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedFastMarketsChoices, actualFastMarketsChoices);
        }

        private Dictionary<string, string> FakeFastMarketsChoices()
        {
            var expectedFastMarketsChoices = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var fastMarketsChoiceInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetFastMarketsChoice(
                        Arg.Is(_betTypeId),
                        Arg.Is(_betTeam),
                        Arg.Is(_handicap1),
                        Arg.Is(_handicap2),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(fastMarketsChoiceInSpecificLanguage);
                expectedFastMarketsChoices.Add(standardLanguageCode, fastMarketsChoiceInSpecificLanguage);
            }

            return expectedFastMarketsChoices;
        }
    }
}