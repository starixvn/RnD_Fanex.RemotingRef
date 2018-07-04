namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetRouletteBetItemSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"RouletteBetItem?itemId={itemId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetRouletteBetItemReturnsValueGotFromService()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();
            var rouletteBetItemName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, rouletteBetItemName } });

            // Act.
            var actualRouletteBetItemName = _remotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(rouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void GetRouletteBetItemWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();
            var defaultRouletteBetItemName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = _remotingRefService.GetRouletteBetItem(
                itemId,
                TestingLanguageCode,
                defaultRouletteBetItemName);

            // Assert.
            Assert.Equal(defaultRouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void GetRouletteBetItemWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = _remotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualRouletteBetItemName);
        }
    }
}