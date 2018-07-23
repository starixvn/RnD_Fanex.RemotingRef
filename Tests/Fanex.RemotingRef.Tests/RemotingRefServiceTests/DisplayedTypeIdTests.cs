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
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedTypeId?betTypeId={betTypeId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ReturnProvidedBetTypeIdIfServiceReturnsNull()
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
    }
}