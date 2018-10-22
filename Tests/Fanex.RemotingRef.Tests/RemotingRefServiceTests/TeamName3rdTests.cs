namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var team3rdId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"TeamName3rd?team3rdId={team3rdId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var team3rdId = Fixture.Create<string>();
            var teamName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = teamName3rd });

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName3rd, actualTeamName3rd);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var team3rdId = Fixture.Create<string>();
            var defaultTeamName3rd = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(
                team3rdId,
                TestingLanguageCode,
                defaultTeamName3rd);

            // Assert.
            Assert.Equal(defaultTeamName3rd, actualTeamName3rd);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableTeam3rdId(string unacceptableTeam3rdId)
        {
            // Arrange.
            var defaultTeamName3rd = Fixture.Create<string>();

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(
                unacceptableTeam3rdId,
                TestingLanguageCode,
                defaultTeamName3rd);

            // Assert.
            Assert.Equal(defaultTeamName3rd, actualTeamName3rd);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableTeam3rdId = string.Empty;

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(unacceptableTeam3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName3rd);
        }
    }
}