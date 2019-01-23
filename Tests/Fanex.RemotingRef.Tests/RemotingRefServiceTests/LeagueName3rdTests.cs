namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LeagueName3rdTests : RemotingRefServiceTestBase
    {
        private readonly string _league3rdId;

        public LeagueName3rdTests()
        {
            _league3rdId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"LeagueName3rd?league3rdId={_league3rdId}");
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
            RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var leagueName3rd = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(leagueName3rd));

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(_league3rdId, TestingLanguageCode);

            // Assert.
            Assert.Equal(leagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultLeagueName3rd = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(
                _league3rdId,
                TestingLanguageCode,
                defaultLeagueName3rd);

            // Assert.
            Assert.Equal(defaultLeagueName3rd, actualLeagueName3rd);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableLeagueId(string unacceptableLeagueId)
        {
            // Arrange.
            var defaultLeagueName3rd = Fixture.Create<string>();

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(
                unacceptableLeagueId,
                TestingLanguageCode,
                defaultLeagueName3rd);

            // Assert.
            Assert.Equal(defaultLeagueName3rd, actualLeagueName3rd);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableLeagueId = string.Empty;

            // Act.
            var actualLeagueName3rd = RemotingRefService.GetLeagueName3rd(unacceptableLeagueId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLeagueName3rd);
        }
    }
}