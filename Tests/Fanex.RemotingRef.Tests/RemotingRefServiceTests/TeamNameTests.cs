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
        public void GetTeamNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var teamId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"TeamName?teamId={teamId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetTeamNameReturnsValueGotFromService()
        {
            // Arrange.
            var teamId = _fixture.Create<string>();
            var teamName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, teamName } });

            // Act.
            var actualTeamName = _remotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName, actualTeamName);
        }

        [Fact]
        public void GetTeamNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var teamId = _fixture.Create<string>();
            var defaultTeamName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = _remotingRefService.GetTeamName(
                teamId,
                TestingLanguageCode,
                defaultTeamName);

            // Assert.
            Assert.Equal(defaultTeamName, actualTeamName);
        }

        [Fact]
        public void GetTeamNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var teamId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = _remotingRefService.GetTeamName(teamId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName);
        }
    }
}