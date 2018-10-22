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
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"TeamName?teamId={teamId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
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
        public void ReturnDefaultValueWhenServiceReturnsNull()
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableTeamId(string unacceptableTeamId)
        {
            // Arrange.
            var defaultTeamName = Fixture.Create<string>();

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                unacceptableTeamId,
                TestingLanguageCode,
                defaultTeamName);

            // Assert.
            Assert.Equal(defaultTeamName, actualTeamName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableTeamId = string.Empty;

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(unacceptableTeamId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}