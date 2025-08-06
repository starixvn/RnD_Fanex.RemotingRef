namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RefControllerTestBase
    {
        private readonly string _itemId;

        public CasinoBetItemTests()
            => _itemId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetCasinoBetItemOfRefDataHelper()
        {
            // Act.
            RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetCasinoBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetCasinoBetItem(Arg.Is(_itemId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetCasinoBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCasinoBetItems();

            // Act.
            var actualResult = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedCasinoBetItems = FakeCasinoBetItems();

            // Act.
            var actualResult = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCasinoBetItems, actualResult.Values);
        }

        private List<string> FakeCasinoBetItems()
        {
            var expectedCasinoBetItems = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var casinoBetItemInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetCasinoBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode))
                    .Returns(casinoBetItemInSpecificLanguage);

                expectedCasinoBetItems.Add(casinoBetItemInSpecificLanguage);
            }

            return expectedCasinoBetItems;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeCasinoBetItemsWithSpecialValue(null);

            // Act.
            var actualResult = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeCasinoBetItemsWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeCasinoBetItemsWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetCasinoBetItem(Arg.Is(_itemId), Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}