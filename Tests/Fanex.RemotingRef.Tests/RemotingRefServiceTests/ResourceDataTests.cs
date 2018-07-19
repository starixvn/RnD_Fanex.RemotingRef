namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultResourceData = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"ResourceData?refId={refId}&resourceId={resourceId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var resourceData = Fixture.Create<string>();
            var defaultResourceData = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = resourceData });

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            Assert.Equal(resourceData, actualResourceData);
        }

        [Fact]
        public void ReturnDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var refId = Fixture.Create<string>();
            var resourceId = Fixture.Create<string>();
            var defaultResourceData = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguageCode,
                defaultResourceData);

            // Assert.
            Assert.Equal(defaultResourceData, actualResourceData);
        }
    }
}