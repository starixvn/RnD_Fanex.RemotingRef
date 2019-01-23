namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
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

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"ResourceData?refId={_refId}&resourceId={_resourceId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
                _resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var resourceData = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(resourceData));

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
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
                _resourceId,
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
            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                _refId,
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
            // Act.
            var actualResourceData = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                invalidRefId,
                _resourceId,
                TestingLanguageCode,
                _defaultResourceData);

            // Assert.
            Assert.Equal(_defaultResourceData, actualResourceData);
        }
    }
}