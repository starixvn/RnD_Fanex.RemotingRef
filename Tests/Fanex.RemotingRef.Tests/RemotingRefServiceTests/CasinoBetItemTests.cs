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
        public void GetCasinoBetItemSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetCasinoBetItem(itemId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"CasinoBetItem?itemId={itemId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetCasinoBetItemReturnsValueGotFromService()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();
            var casinoBetItemName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, casinoBetItemName } });

            // Act.
            var actualCasinoBetItemName = _remotingRefService.GetCasinoBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(casinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void GetCasinoBetItemWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();
            var defaultCasinoBetItemName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCasinoBetItemName = _remotingRefService.GetCasinoBetItem(
                itemId,
                TestingLanguageCode,
                defaultCasinoBetItemName);

            // Assert.
            Assert.Equal(defaultCasinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void GetCasinoBetItemWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var itemId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCasinoBetItemName = _remotingRefService.GetCasinoBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCasinoBetItemName);
        }
    }
}