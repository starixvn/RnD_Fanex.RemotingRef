namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class HorseTeamNameTests : RefControllerBaseTests
    {
        private readonly string _codeId;

        public HorseTeamNameTests()
            => _codeId = Fixture.Create<string>();

        [Fact]
        public void CallGetHorseTeamNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetHorseTeamName(Arg.Is(_codeId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetHorseTeamNameOfRefDataHelper()
        {
            // Act.
            RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                .Received()
                .GetHorseTeamName(Arg.Is(_codeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeHorseTeamNames();

            // Act.
            var actualHorseTeamNames = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualHorseTeamNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetHorseTeamNameOfRefDataHelper()
        {
            // Arrange.
            var expectedHorseTeamNames = FakeHorseTeamNames();

            // Act.
            var actualHorseTeamNames = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedHorseTeamNames, actualHorseTeamNames);
        }

        private Dictionary<string, string> FakeHorseTeamNames()
        {
            var expectedHorseTeamNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var horseTeamNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetHorseTeamName(Arg.Is(_codeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(horseTeamNameInSpecificLanguage);
                expectedHorseTeamNames.Add(standardLanguageCode, horseTeamNameInSpecificLanguage);
            }

            return expectedHorseTeamNames;
        }
    }
}