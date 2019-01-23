namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TeamNameTests : RemotingRefServiceTestBase
    {
        private readonly string _teamId;

        public TeamNameTests()
        {
            _teamId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"TeamName?teamId={_teamId}");
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
            RemotingRefService.GetTeamName(_teamId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var teamName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(teamName));

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(_teamId, TestingLanguageCode);

            // Assert.
            Assert.Equal(teamName, actualTeamName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultTeamName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(
                _teamId,
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