namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsResultTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var resultId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetFastMarketsResult(resultId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"FastMarketsResult?resultId={resultId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var resultId = Fixture.Create<string>();
            var result = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = result });

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            var resultId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguageCode);

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