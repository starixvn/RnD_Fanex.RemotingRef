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
        public void GetParentIdByBetTypeIdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var defaultParentTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ParentIdByBetTypeId?betTypeId={betTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetParentIdByBetTypeIdReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var parentTypeId = _fixture.Create<string>();
            var defaultParentTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, parentTypeId } });

            // Act.
            var actualParentTypeId = _remotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            Assert.Equal(parentTypeId, actualParentTypeId);
        }

        [Fact]
        public void GetParentIdByBetTypeIdReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var defaultParentTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualParentTypeId = _remotingRefService.GetParentIdByBetTypeId(betTypeId, defaultParentTypeId);

            // Assert.
            Assert.Equal(defaultParentTypeId, actualParentTypeId);
        }
    }
}