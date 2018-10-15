namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RemotingRefServiceTestBase
    {
        private readonly string _itemId;

        public RouletteBetItemTests()
            => _itemId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetRouletteBetItem(_itemId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"RouletteBetItem?itemId={_itemId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var rouletteBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = rouletteBetItemName });

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(rouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultRouletteBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(
                _itemId,
                TestingLanguageCode,
                defaultRouletteBetItemName);

            // Assert.
            Assert.Equal(defaultRouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualRouletteBetItemName);
        }
    }
}