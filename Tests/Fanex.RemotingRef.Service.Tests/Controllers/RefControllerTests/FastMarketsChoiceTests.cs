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
        public void AlwaysCallGetTeamNameOfRefDataHelper()
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
                .Received()
                .GetFastMarketsChoice(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
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
        public void CallRefDataHelperWithCorrectArguments()
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
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarketsChoices();

            // Act.
            var actualResult = RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedFastMarketsChoices = FakeFastMarketsChoices();

            // Act.
            var actualResult = RefController.FastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedFastMarketsChoices, actualResult.Values);
        }

        private List<string> FakeFastMarketsChoices()
        {
            var expectedFastMarketsChoices = new List<string>();

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
                expectedFastMarketsChoices.Add(fastMarketsChoiceInSpecificLanguage);
            }

            return expectedFastMarketsChoices;
        }
    }
}