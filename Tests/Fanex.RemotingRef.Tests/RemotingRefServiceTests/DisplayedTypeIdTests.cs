namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"DisplayedTypeId?betTypeId={betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();
            var displayTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayTypeId });

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            Assert.Equal(displayTypeId, actualDisplayTypeId);
        }

        [Fact]
        public void ReturnProvidedBetTypeIdWhenServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            Assert.Equal(betTypeId, actualDisplayTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnProvidedBetTypeIdWhenItIsUnacceptable(string unacceptableBetTypeId)
        {
            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(unacceptableBetTypeId);

            // Assert.
            Assert.Equal(unacceptableBetTypeId, actualDisplayTypeId);
        }
    }
}