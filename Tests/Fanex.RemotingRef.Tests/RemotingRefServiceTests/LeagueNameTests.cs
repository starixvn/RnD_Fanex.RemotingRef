namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RemotingRefServiceTestBase
    {
        private readonly string _leagueId;

        public LeagueNameTests()
            => _leagueId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetLeagueName(_leagueId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName?leagueId={_leagueId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = leagueName });

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultLeagueName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                _leagueId,
                TestingLanguageCode,
                defaultLeagueName);

            // Assert.
            Assert.Equal(defaultLeagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName);
        }
    }
}