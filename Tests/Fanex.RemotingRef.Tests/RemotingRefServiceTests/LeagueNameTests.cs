namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var leagueId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"LeagueName?leagueId={leagueId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueId = Fixture.Create<string>();
            var leagueName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = leagueName });

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var leagueId = Fixture.Create<string>();
            var defaultLeagueName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                leagueId,
                TestingLanguageCode,
                defaultLeagueName);

            // Assert.
            Assert.Equal(defaultLeagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var leagueId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName);
        }
    }
}