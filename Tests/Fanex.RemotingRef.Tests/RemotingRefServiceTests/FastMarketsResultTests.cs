namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarketsResultTests : RemotingRefServiceTestBase
    {
        private readonly string _resultId;

        public FastMarketsResultTests()
            => _resultId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarketsResult?resultId={_resultId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var result = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = result });

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(_resultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}