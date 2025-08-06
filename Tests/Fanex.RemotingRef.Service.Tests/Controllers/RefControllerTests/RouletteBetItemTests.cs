namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RefControllerTestBase
    {
        private readonly string _itemId;

        public RouletteBetItemTests()
            => _itemId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetRouletteBetItemOfRefDataHelper()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetRouletteBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetRouletteBetItem(Arg.Is(_itemId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeRouletteBetItems();

            // Act.
            var actualResult = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedRouletteBetItems = FakeRouletteBetItems();

            // Act.
            var actualResult = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedRouletteBetItems, actualResult.Values);
        }

        private List<string> FakeRouletteBetItems()
        {
            var expectedRouletteBetItems = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var rouletteBetItemInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode))
                    .Returns(rouletteBetItemInSpecificLanguage);

                expectedRouletteBetItems.Add(rouletteBetItemInSpecificLanguage);
            }

            return expectedRouletteBetItems;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeRouletteBetItemsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeRouletteBetItemsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeRouletteBetItemsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}