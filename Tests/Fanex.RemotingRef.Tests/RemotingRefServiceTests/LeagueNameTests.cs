namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueNameTests : RemotingRefServiceTestBase
    {
        private readonly string _leagueId;

        public LeagueNameTests()
        {
            _leagueId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"LeagueName?leagueId={_leagueId}");
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
            RemotingRefService.GetLeagueName(_leagueId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(leagueName));

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(_leagueId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName, actualLeagueName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultLeagueName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(
                _leagueId,
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