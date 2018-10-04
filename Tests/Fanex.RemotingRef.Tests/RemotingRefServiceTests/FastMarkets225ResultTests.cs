namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var betType255ResultId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarkets225Result?resultId={betType255ResultId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betType255ResultId = Fixture.Create<string>();
            var result = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = result });

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var betType255ResultId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}