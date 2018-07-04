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
        public void GetDisplayedTypeIdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var defaultDisplayTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetDisplayedTypeId(betTypeId, defaultDisplayTypeId);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"DisplayedTypeId?betTypeId={betTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetDisplayedTypeIdReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var displayTypeId = _fixture.Create<string>();
            var defaultDisplayTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, displayTypeId } });

            // Act.
            var actualDisplayTypeId = _remotingRefService.GetDisplayedTypeId(betTypeId, defaultDisplayTypeId);

            // Assert.
            Assert.Equal(displayTypeId, actualDisplayTypeId);
        }

        [Fact]
        public void GetDisplayedTypeIdReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var defaultDisplayTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualDisplayTypeId = _remotingRefService.GetDisplayedTypeId(betTypeId, defaultDisplayTypeId);

            // Assert.
            Assert.Equal(defaultDisplayTypeId, actualDisplayTypeId);
        }
    }
}