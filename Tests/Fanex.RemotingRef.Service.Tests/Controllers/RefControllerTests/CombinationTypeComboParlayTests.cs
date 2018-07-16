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
        public void CallGetCombinationTypeComboOfRefDataHelperOncePerLanguage()
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
        public void SendCorrectArgumentsToGetCombinationTypeComboOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCombinationTypeCombos();

            // Act.
            var actualCombinationTypeCombos = RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualCombinationTypeCombos.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetCombinationTypeComboOfRefDataHelper()
        {
            // Arrange.
            var expectedCombinationTypeCombos = FakeCombinationTypeCombos();

            // Act.
            var actualCombinationTypeCombos = RefController.CombinationTypeComboParlay(
                _betTeam,
                _betCheck,
                _handicap1,
                _handicap2,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCombinationTypeCombos, actualCombinationTypeCombos);
        }

        private Dictionary<string, string[]> FakeCombinationTypeCombos()
        {
            var expectedCombinationTypeCombos = new Dictionary<string, string[]>();

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
                expectedCombinationTypeCombos.Add(standardLanguageCode, comboInSpecificLanguage);
            }

            return expectedCombinationTypeCombos;
        }
    }
}