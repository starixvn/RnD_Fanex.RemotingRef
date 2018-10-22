namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RemotingRefServiceTestBase
    {
        private readonly string _defaultResourceData;

        public ResourceDataTests()
            => _defaultResourceData = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"ResourceData?refId={refId}&resourceId={resourceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var resourceData = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = resourceData });

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(resourceData, actualResourceData);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(_defaultResourceData, actualResourceData);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnDefaultValueForNullOrEmptyRefId(string invalidResourceId)
        {
            // Arrange.
            var refId = Fixture.Create<string>();

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                invalidResourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(_defaultResourceData, actualResourceData);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnDefaultValueForNullOrEmptyResourceId(string invalidRefId)
        {
            // Arrange.
            var resourceId = Fixture.Create<string>();

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                invalidRefId,
                resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(_defaultResourceData, actualResourceData);
        }
    }
}