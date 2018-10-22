namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var leagueId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLeagueName(leagueId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"LeagueName?leagueId={leagueId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
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
        public void ReturnDefaultValueWhenServiceReturnsNull()
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableLeagueId(string unacceptableLeagueId)
        {
            // Arrange.
            var defaultLeagueName = Fixture.Create<string>();

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                unacceptableLeagueId,
                TestingLanguageCode,
                defaultLeagueName);

            // Assert.
            Assert.Equal(defaultLeagueName, actualLeagueName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableLeagueId = string.Empty;

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(unacceptableLeagueId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName);
        }
    }
}