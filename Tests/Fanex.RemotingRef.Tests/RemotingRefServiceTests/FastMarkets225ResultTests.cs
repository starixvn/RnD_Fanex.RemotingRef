namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RemotingRefServiceTestBase
    {
        private readonly string _betType255ResultId;

        public FastMarkets225ResultTests()
        {
            _betType255ResultId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"FastMarkets225Result?resultId={_betType255ResultId}");
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
            RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var result = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(result));

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnEmptyStringForUnacceptableResultId(string unacceptableBetType255ResultId)
        {
            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(unacceptableBetType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}