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
        public void CallGetRouletteBetItemOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetRouletteBetItem(Arg.Is(_itemId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetRouletteBetItemOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeRouletteBetItems();

            // Act.
            var actualRouletteBetItems = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualRouletteBetItems.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetRouletteBetItemOfRefDataHelper()
        {
            // Arrange.
            var expectedRouletteBetItems = FakeRouletteBetItems();

            // Act.
            var actualRouletteBetItems = RefController.RouletteBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedRouletteBetItems, actualRouletteBetItems);
        }

        private Dictionary<string, string> FakeRouletteBetItems()
        {
            var expectedRouletteBetItems = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var rouletteBetItemInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetRouletteBetItem(Arg.Is(_itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(rouletteBetItemInSpecificLanguage);
                expectedRouletteBetItems.Add(standardLanguageCode, rouletteBetItemInSpecificLanguage);
            }

            return expectedRouletteBetItems;
        }
    }
}