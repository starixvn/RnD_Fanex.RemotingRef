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
        public void GetFastMarketsChoiceSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var betTeam = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();

            // Act.
            _remotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarketsChoice?betTypeId={betTypeId}&betTeam={betTeam}&handicap1={handicap1}&handicap2={handicap2}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetFastMarketsChoiceReturnsValueGotFromService()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var betTeam = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();
            var fastMarketsChoice = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, fastMarketsChoice } });

            // Act.
            var actualFastMarketsChoice = _remotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(fastMarketsChoice, actualFastMarketsChoice);
        }

        [Fact]
        public void GetFastMarketsChoiceReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var betTypeId = _fixture.Create<string>();
            var betTeam = _fixture.Create<string>();
            var handicap1 = _fixture.Create<int>();
            var handicap2 = _fixture.Create<int>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualFastMarketsChoice = _remotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualFastMarketsChoice);
        }

        [Fact]
        public void GetFastMarkets225ResultSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var resultId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetFastMarkets225Result(resultId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"FastMarkets225Result?resultId={resultId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetFastMarkets225ResultReturnsValueGotFromService()
        {
            // Arrange.
            var resultId = _fixture.Create<string>();
            var result = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, result } });

            // Act.
            var actualResult = _remotingRefService.GetFastMarkets225Result(resultId, TestingLanguageCode);

            // Assert.
            Assert.Equal(result, actualResult);
        }

        [Fact]
        public void GetFastMarkets225ResultReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var resultId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualResult = _remotingRefService.GetFastMarkets225Result(resultId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualResult);
        }
    }
}