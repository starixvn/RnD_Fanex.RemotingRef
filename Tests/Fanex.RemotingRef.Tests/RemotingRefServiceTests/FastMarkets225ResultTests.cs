namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RemotingRefServiceTestBase
    {
        private readonly string _betType255ResultId;

        public FastMarkets225ResultTests()
            => _betType255ResultId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarkets225Result?resultId={_betType255ResultId}");

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
            var actualResult = RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

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
            var actualResult = RemotingRefService.GetFastMarkets225Result(_betType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}