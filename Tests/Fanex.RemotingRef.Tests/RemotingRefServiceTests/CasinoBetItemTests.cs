namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RemotingRefServiceTestBase
    {
        private readonly string _itemId;

        public CasinoBetItemTests()
            => _itemId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetCasinoBetItem(_itemId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"CasinoBetItem?itemId={_itemId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var casinoBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = casinoBetItemName });

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(casinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultCasinoBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(
                _itemId,
                TestingLanguageCode,
                defaultCasinoBetItemName);

            // Assert.
            Assert.Equal(defaultCasinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(_itemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCasinoBetItemName);
        }
    }
}