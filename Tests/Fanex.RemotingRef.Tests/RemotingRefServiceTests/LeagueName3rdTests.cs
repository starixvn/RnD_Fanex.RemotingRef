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
        public void GetLeagueName3rdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var league3rdId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName3rd?league3rdId={league3rdId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetLeagueName3rdReturnsValueGotFromService()
        {
            // Arrange.
            var league3rdId = _fixture.Create<string>();
            var leagueName3rd = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, leagueName3rd } });

            // Act.
            var actualLeagueName3rd = _remotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void GetLeagueName3rdWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var league3rdId = _fixture.Create<string>();
            var defaultLeagueName3rd = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = _remotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode, defaultLeagueName3rd);

            // Assert.
            Assert.Equal(defaultLeagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void GetLeagueName3rdWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var league3rdId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName3rd = _remotingRefService.GetLeagueName3rd(league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName3rd);
        }
    }
}