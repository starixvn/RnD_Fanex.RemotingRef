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
        public void GetTeamName3rdSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var team3rdId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"TeamName3rd?team3rdId={team3rdId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetTeamName3rdReturnsValueGotFromService()
        {
            // Arrange.
            var team3rdId = _fixture.Create<string>();
            var teamName3rd = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, teamName3rd } });

            // Act.
            var actualTeamName3rd = _remotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName3rd, actualTeamName3rd);
        }

        [Fact]
        public void GetTeamName3rdWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var team3rdId = _fixture.Create<string>();
            var defaultTeamName3rd = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName3rd = _remotingRefService.GetTeamName3rd(
                team3rdId,
                TestingLanguageCode,
                defaultTeamName3rd);

            // Assert.
            Assert.Equal(defaultTeamName3rd, actualTeamName3rd);
        }

        [Fact]
        public void GetTeamName3rdWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var team3rdId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName3rd = _remotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName3rd);
        }
    }
}