namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetBetRadarMatchCodeFormatSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var matchCode = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"BetRadarMatchCodeFormat?matchCode={matchCode}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetBetRadarMatchCodeFormatReturnsValueGotFromService()
        {
            // Arrange.
            var matchCode = _fixture.Create<string>();
            var matchFormat = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, matchFormat } });

            // Act.
            var actualMatchFormat = _remotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchFormat, actualMatchFormat);
        }

        [Fact]
        public void GetBetRadarMatchCodeFormatReturnsMatchCodeIfServiceReturnsNull()
        {
            // Arrange.
            var matchCode = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualMatchFormat = _remotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(matchCode, actualMatchFormat);
        }
    }
}