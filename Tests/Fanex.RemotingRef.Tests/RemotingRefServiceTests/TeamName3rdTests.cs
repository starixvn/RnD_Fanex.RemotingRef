namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var team3rdId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"TeamName3rd?team3rdId={team3rdId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
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
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
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

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var team3rdId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(team3rdId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualTeamName3rd);
        }
    }
}