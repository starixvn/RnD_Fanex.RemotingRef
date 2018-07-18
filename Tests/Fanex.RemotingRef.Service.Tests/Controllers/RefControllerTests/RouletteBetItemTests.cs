namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RefControllerBaseTests
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
            RefData.Received().GetRouletteBetItem(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetRouletteBetItem(Arg.Is(_itemId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
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
                    .GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(rouletteBetItemInSpecificLanguage);
                expectedRouletteBetItems.Add(rouletteBetItemInSpecificLanguage);
            }

            return expectedRouletteBetItems;
        }
    }
}