namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RemotingRefServiceTestBase
    {
        private readonly string _teamId;

        public TeamNameTests()
            => _teamId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetTeamName(_teamId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"TeamName?teamId={_teamId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var teamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = teamName });

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(_teamId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName, actualTeamName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                _teamId,
                TestingLanguageCode,
                defaultTeamName);

            // Assert.
            Assert.Equal(defaultTeamName, actualTeamName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(_teamId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}