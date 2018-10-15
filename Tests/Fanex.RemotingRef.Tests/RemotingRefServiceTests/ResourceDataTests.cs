namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RemotingRefServiceTestBase
    {
        private readonly string _refId;
        private readonly string _resourceId;
        private readonly string _defaultResourceData;

        public ResourceDataTests()
        {
            _refId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _defaultResourceData = Fixture.Create<string>();
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
                _resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ResourceData?refId={_refId}&resourceId={_resourceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var resourceData = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = resourceData });

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
                _resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(resourceData, actualResourceData);
        }

        [Fact]
        public void ReturnDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
                _resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(_defaultResourceData, actualResourceData);
        }
    }
}