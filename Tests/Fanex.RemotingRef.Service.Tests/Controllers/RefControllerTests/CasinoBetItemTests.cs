namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RefControllerBaseTests
    {
        private readonly string _itemId;

        public CasinoBetItemTests()
            => _itemId = Fixture.Create<string>();

        [Fact]
        public void CallGetCasinoBetItemOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetCasinoBetItem(Arg.Is(_itemId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetCasinoBetItemOfRefDataHelper()
        {
            // Act.
            RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetCasinoBetItem(Arg.Is(_itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeCasinoBetItems();

            // Act.
            var actualCasinoBetItems = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualCasinoBetItems.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetCasinoBetItemOfRefDataHelper()
        {
            // Arrange.
            var expectedCasinoBetItems = FakeCasinoBetItems();

            // Act.
            var actualCasinoBetItems = RefController.CasinoBetItem(_itemId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedCasinoBetItems, actualCasinoBetItems);
        }

        private Dictionary<string, string> FakeCasinoBetItems()
        {
            var expectedCasinoBetItems = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var casinoBetItemInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetCasinoBetItem(Arg.Is(_itemId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(casinoBetItemInSpecificLanguage);
                expectedCasinoBetItems.Add(standardLanguageCode, casinoBetItemInSpecificLanguage);
            }

            return expectedCasinoBetItems;
        }
    }
}