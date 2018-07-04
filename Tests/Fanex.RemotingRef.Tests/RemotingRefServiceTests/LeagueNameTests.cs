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
        public void GetLeagueNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var leagueId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName?leagueId={leagueId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetLeagueNameReturnsValueGotFromService()
        {
            // Arrange.
            var leagueId = _fixture.Create<string>();
            var leagueName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, leagueName } });

            // Act.
            var actualLeagueName = _remotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Fact]
        public void GetLeagueNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var leagueId = _fixture.Create<string>();
            var defaultLeagueName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = _remotingRefService.GetLeagueName(
                leagueId,
                TestingLanguageCode,
                defaultLeagueName);

            // Assert.
            Assert.Equal(defaultLeagueName, actualLeagueName);
        }

        [Fact]
        public void GetLeagueNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var leagueId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = _remotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName);
        }
    }
}