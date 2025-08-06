namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class HorseTeamNameTests : RefControllerTestBase
    {
        private readonly string _codeId;

        public HorseTeamNameTests()
            => _codeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetHorseTeamNameOfRefDataHelper()
        {
            // Act.
            RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetHorseTeamName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetHorseTeamName(Arg.Is(_codeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetHorseTeamName(Arg.Is(_codeId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeHorseTeamNames();

            // Act.
            var actualResult = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedHorseTeamNames = FakeHorseTeamNames();

            // Act.
            var actualResult = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedHorseTeamNames, actualResult.Values);
        }

        private List<string> FakeHorseTeamNames()
        {
            var expectedHorseTeamNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var horseTeamNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetHorseTeamName(Arg.Is(_codeId), Arg.Is(standardLanguageCode))
                    .Returns(horseTeamNameInSpecificLanguage);

                expectedHorseTeamNames.Add(horseTeamNameInSpecificLanguage);
            }

            return expectedHorseTeamNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeHorseTeamNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeHorseTeamNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.HorseTeamName(_codeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeHorseTeamNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetHorseTeamName(Arg.Is(_codeId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}