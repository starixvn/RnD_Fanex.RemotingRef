namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CombinationTypeComboParlayTests : RefControllerBaseTests
    {
        private readonly string _betTeam;
        private readonly string _betCheck;
        private readonly int _handicap1;
        private readonly int _handicap2;

        public CombinationTypeComboParlayTests()
        {
            _betTeam = Fixture.Create<string>();
            _betCheck = Fixture.Create<string>();
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();
        }

        [Fact]
        public void AlwaysCallGetTeamNameOfRefDataHelper()
        {
            // Act.
            RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetCombinationTypeCombo(
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
            RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetCombinationTypeCombo(
                    Arg.Is(_betTeam),
                    Arg.Is(_betCheck),
                    Arg.Is(_handicap1),
                    Arg.Is(_handicap2),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                .Received()
                .GetCombinationTypeCombo(
                    Arg.Is(_betTeam),
                    Arg.Is(_betCheck),
                    Arg.Is(_handicap1),
                    Arg.Is(_handicap2),
                    Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCombinationTypeCombos();

            // Act.
            var actualResult = RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
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
            var expectedCombinationTypeCombos = FakeCombinationTypeCombos();

            // Act.
            var actualResult = RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCombinationTypeCombos, actualResult.Values);
        }

        private List<string[]> FakeCombinationTypeCombos()
        {
            var expectedCombinationTypeCombos = new List<string[]>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var comboInSpecificLanguage = Fixture.Create<string[]>();

                RefData
                    .GetCombinationTypeCombo(
                        Arg.Is(_betTeam),
                        Arg.Is(_betCheck),
                        Arg.Is(_handicap1),
                        Arg.Is(_handicap2),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(comboInSpecificLanguage);
                expectedCombinationTypeCombos.Add(comboInSpecificLanguage);
            }

            return expectedCombinationTypeCombos;
        }
    }
}