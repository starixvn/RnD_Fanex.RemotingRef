namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;

        public DisplayedTypeIdTests()
            => _betTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedTypeId?betTypeId={_betTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var displayTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = displayTypeId });

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(displayTypeId, actualDisplayTypeId);
        }

        [Fact]
        public void ReturnProvidedBetTypeIdIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(_betTypeId, actualDisplayTypeId);
        }
    }
}