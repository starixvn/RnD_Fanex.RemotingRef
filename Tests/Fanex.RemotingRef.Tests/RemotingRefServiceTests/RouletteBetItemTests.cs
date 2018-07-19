namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"RouletteBetItem?itemId={itemId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();
            var rouletteBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = rouletteBetItemName });

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(rouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();
            var defaultRouletteBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(
                itemId,
                TestingLanguageCode,
                defaultRouletteBetItemName);

            // Assert.
            Assert.Equal(defaultRouletteBetItemName, actualRouletteBetItemName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualRouletteBetItemName = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualRouletteBetItemName);
        }
    }
}