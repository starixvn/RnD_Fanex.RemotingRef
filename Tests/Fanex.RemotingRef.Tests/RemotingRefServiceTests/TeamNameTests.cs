namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var teamId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"TeamName?teamId={teamId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var teamId = Fixture.Create<string>();
            var teamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = teamName });

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName, actualTeamName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var teamId = Fixture.Create<string>();
            var defaultTeamName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                teamId,
                TestingLanguageCode,
                defaultTeamName);

            // Assert.
            Assert.Equal(defaultTeamName, actualTeamName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var teamId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}