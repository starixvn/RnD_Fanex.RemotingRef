namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _betTeam;
        private readonly int _handicap1;
        private readonly int _handicap2;

        public FastMarketsChoiceTests()
        {
            _betTypeId = Fixture.Create<string>();
            _betTeam = Fixture.Create<string>();
            _handicap1 = Fixture.Create<int>();
            _handicap2 = Fixture.Create<int>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"FastMarketsChoice?betTypeId={_betTypeId}&betTeam={_betTeam}&handicap1={_handicap1}&handicap2={_handicap2}");
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
            RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var fastMarketsChoice = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(fastMarketsChoice));

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(fastMarketsChoice, actualFastMarketsChoice);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnEmptyStringForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                unacceptableBetTypeId,
                _betTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnEmptyStringForNullOrEmptyBetTeam(string invalidBetTeam)
        {
            // Act.
            var actualFastMarketsChoice = RemotingRefService.GetFastMarketsChoice(
                _betTypeId,
                invalidBetTeam,
                _handicap1,
                _handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }
    }
}