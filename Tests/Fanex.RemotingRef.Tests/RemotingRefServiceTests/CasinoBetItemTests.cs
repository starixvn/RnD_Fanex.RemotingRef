namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CasinoBetItemTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetCasinoBetItem(itemId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"CasinoBetItem?itemId={itemId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();
            var casinoBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = casinoBetItemName });

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(itemId, TestingLanguageCode);

            // Assert.
            Assert.Equal(casinoBetItemName, actualCasinoBetItemName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var itemId = Fixture.Create<string>();
            var defaultCasinoBetItemName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCasinoBetItemName = RemotingRefService.GetCasinoBetItem(
                itemId,
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