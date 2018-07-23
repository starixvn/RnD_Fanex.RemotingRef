namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var matchCode = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetRadarMatchCodeFormat?matchCode={matchCode}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnMatchCodeIfServiceReturnsNull()
        {
            // Arrange.
            var matchCode = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchCode, actualMatchFormat);
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var matchCode = Fixture.Create<string>();
            var matchFormat = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = matchFormat });

            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchFormat, actualMatchFormat);
        }
    }
}