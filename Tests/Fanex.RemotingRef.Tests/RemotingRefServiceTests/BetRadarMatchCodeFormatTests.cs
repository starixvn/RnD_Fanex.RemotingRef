namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RemotingRefServiceTestBase
    {
        private readonly string _matchCode;

        public BetRadarMatchCodeFormatTests()
            => _matchCode = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetRadarMatchCodeFormat?matchCode={_matchCode}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnMatchCodeIfServiceReturnsNull()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(_matchCode, actualMatchFormat);
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var matchFormat = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = matchFormat });

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(_matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchFormat, actualMatchFormat);
        }
    }
}