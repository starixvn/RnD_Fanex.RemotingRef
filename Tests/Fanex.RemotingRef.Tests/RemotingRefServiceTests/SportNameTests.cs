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
        public void GetSportNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var sportTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"SportName?sportTypeId={sportTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetSportNameReturnsValueGotFromService()
        {
            // Arrange.
            var sportTypeId = _fixture.Create<string>();
            var sportTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, sportTypeName } });

            // Act.
            var actualSportName = _remotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(sportTypeName, actualSportName);
        }

        [Fact]
        public void GetSportNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var sportTypeId = _fixture.Create<string>();
            var defaultSportTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = _remotingRefService.GetSportName(sportTypeId, TestingLanguageCode, defaultSportTypeName);

            // Assert.
            Assert.Equal(defaultSportTypeName, actualSportName);
        }

        [Fact]
        public void GetSportNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var sportTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualSportName = _remotingRefService.GetSportName(sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSportName);
        }
    }
}