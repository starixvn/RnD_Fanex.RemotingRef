namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class RouletteBetItemTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetRouletteBetItem(itemId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"RouletteBetItem?itemId={itemId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
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
        public void ReturnDefaultValueWhenServiceReturnsNull()
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