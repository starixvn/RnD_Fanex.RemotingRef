namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetBuilderSelectionNameTests : RefControllerTestBase
    {
        private readonly int _selectionId;

        public BetBuilderSelectionNameTests()
            => _selectionId = Fixture.Create<int>();

        [Fact]
        public void AlwaysCallGetBetBuilderSelectionNameOfRefDataHelper()
        {
            // Act.
            RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetBetBuilderSelectionName(Arg.Any<int>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetBetBuilderSelectionName(Arg.Is(_selectionId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetBetBuilderSelectionName(Arg.Is(_selectionId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeBetBuilderSelectionNames();

            // Act.
            var actualResult = RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedSelectionNames = FakeBetBuilderSelectionNames();

            // Act.
            var actualResult = RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedSelectionNames, actualResult.Values);
        }

        private List<string> FakeBetBuilderSelectionNames()
        {
            var expectedSelectionNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var selectionNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetBuilderSelectionName(Arg.Is(_selectionId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(selectionNameInSpecificLanguage);

                expectedSelectionNames.Add(selectionNameInSpecificLanguage);
            }

            return expectedSelectionNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeBetBuilderSelectionNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeBetBuilderSelectionNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.BetBuilderSelectionName(_selectionId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeBetBuilderSelectionNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetBetBuilderSelectionName(Arg.Is(_selectionId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}