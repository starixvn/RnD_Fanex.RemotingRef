namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RemotingRefServiceTestBase
    {
        private readonly string _itemId;

        public RouletteBetItemTests()
        {
            _itemId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"RouletteBetItem?itemId={_itemId}");
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
            RemotingRefService.GetRouletteBetItem(_itemId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var rouletteBetItemName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(rouletteBetItemName));

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(rouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultRouletteBetItemName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(
                _itemId,
                TestingLanguageCode,
                defaultRouletteBetItemName);

            // Assert.
            Assert.Equal(defaultRouletteBetItemName, actualRouletteBetItemName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnDefaultValueForUnacceptableItemId(string unacceptableItemId)
        {
            // Arrange.
            var defaultRouletteBetItemName = Fixture.Create<string>();

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(
                unacceptableItemId,
                TestingLanguageCode,
                defaultRouletteBetItemName);

            // Assert.
            Assert.Equal(defaultRouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableItemId = string.Empty;

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(unacceptableItemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualRouletteBetItemName);
        }
    }
}