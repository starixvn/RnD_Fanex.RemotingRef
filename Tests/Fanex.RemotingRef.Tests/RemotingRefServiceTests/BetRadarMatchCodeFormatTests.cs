namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetRadarMatchCodeFormatTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var matchCode = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetRadarMatchCodeFormat?matchCode={matchCode}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnMatchCodeWhenServiceReturnsNull()
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnMatchCodeWhenItIsNullOrEmpty(string unacceptableMatchCode)
        {
            // Act.
            var actualMatchFormat = RemotingRefService.GetBetRadarMatchCodeFormat(unacceptableMatchCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(unacceptableMatchCode, actualMatchFormat);
        }
    }
}