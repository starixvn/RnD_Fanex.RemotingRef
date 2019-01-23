namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RemotingRefServiceTestBase
    {
        private readonly string _matchCode;

        public BetRadarMatchCodeFormatTests()
        {
            _matchCode = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"BetRadarMatchCodeFormat?matchCode={_matchCode}");
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
            RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var matchFormat = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(matchFormat));

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchFormat, actualMatchFormat);
        }

        [Fact]
        public void ReturnMatchCodeWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(_matchCode, actualMatchFormat);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnMatchCodeWhenItIsNullOrEmpty(string unacceptableMatchCode)
        {
            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(unacceptableMatchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(unacceptableMatchCode, actualMatchFormat);
        }
    }
}