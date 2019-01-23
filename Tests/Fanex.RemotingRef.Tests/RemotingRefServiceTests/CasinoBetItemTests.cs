namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RemotingRefServiceTestBase
    {
        private readonly string _itemId;

        public CasinoBetItemTests()
        {
            _itemId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"CasinoBetItem?itemId={_itemId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetCasinoBetItem(_itemId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var casinoBetItemName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(casinoBetItemName));

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(casinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultCasinoBetItemName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(
                _itemId,
                TestingLanguageCode,
                defaultCasinoBetItemName);

            // Assert.
            Assert.Equal(defaultCasinoBetItemName, actualCasinoBetItemName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnDefaultValueForUnacceptableItemId(string unacceptableItemId)
        {
            // Arrange.
            var defaultCasinoBetItemName = Fixture.Create<string>();

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(
                unacceptableItemId,
                TestingLanguageCode,
                defaultCasinoBetItemName);

            // Assert.
            Assert.Equal(defaultCasinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableItemId = string.Empty;

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(unacceptableItemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCasinoBetItemName);
        }
    }
}