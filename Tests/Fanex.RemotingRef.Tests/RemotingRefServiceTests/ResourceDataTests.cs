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
        public void GetResourceDataByRefIdAndResourceIdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var refId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var defaultResourceData = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ResourceData?refId={refId}&resourceId={resourceId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceIdReturnsValueGotFromService()
        {
            // Arrange.
            var refId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var resourceData = _fixture.Create<string>();
            var defaultResourceData = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, resourceData } });

            // Act.
            var actualResourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            Assert.Equal(resourceData, actualResourceData);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceIdReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var refId = _fixture.Create<string>();
            var resourceId = _fixture.Create<string>();
            var defaultResourceData = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            Assert.Equal(defaultResourceData, actualResourceData);
        }
    }
}