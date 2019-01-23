namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsResultTests : RemotingRefServiceTestBase
    {
        private readonly string _resultId;

        public FastMarketsResultTests()
        {
            _resultId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"FastMarketsResult?resultId={_resultId}");
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
            RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

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
            var actualResult = RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnEmptyStringForUnacceptableResultId(string unacceptableResultId)
        {
            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(unacceptableResultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}