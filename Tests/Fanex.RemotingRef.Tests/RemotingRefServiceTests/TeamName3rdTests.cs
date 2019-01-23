namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamName3rdTests : RemotingRefServiceTestBase
    {
        private readonly string _team3rdId;

        public TeamName3rdTests()
        {
            _team3rdId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"TeamName3rd?team3rdId={_team3rdId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetTeamName3rd(_team3rdId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var teamName3rd = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(teamName3rd));

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(_team3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName3rd, actualTeamName3rd);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultTeamName3rd = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualTeamName3rd = RemotingRefService.GetTeamName3rd(
                _team3rdId,
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