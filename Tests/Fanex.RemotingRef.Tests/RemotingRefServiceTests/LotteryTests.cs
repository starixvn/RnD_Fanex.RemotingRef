namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetLotteryBetChoiceSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var lotteryBetChoiceId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetLotteryBetChoice(lotteryBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"LotteryBetChoice?betChoiceId={lotteryBetChoiceId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetLotteryBetChoiceReturnsValueGotFromService()
        {
            // Arrange.
            var lotteryBetChoiceId = _fixture.Create<string>();
            var lotteryBetChoiceName = _fixture.Create<string>();
            var lotteryBetTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>> { { TestingLanguageCode, new Tuple<string, string>(lotteryBetChoiceName, lotteryBetTypeId) } });

            // Act.
            var actualLotteryBetChoiceName = _remotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(lotteryBetChoiceName, actualLotteryBetChoiceName);
            Assert.Equal(lotteryBetTypeId, actualLotteryBetTypeId);
        }

        [Fact]
        public void GetLotteryBetChoiceWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetChoiceId = _fixture.Create<string>();
            var defaultValue = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetChoiceName = _remotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetChoiceName);
            Assert.Equal(defaultValue, actualLotteryBetTypeId);
        }

        [Fact]
        public void GetLotteryBetChoiceWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetChoiceId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = _remotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Empty(actualTeamName);
            Assert.Empty(actualLotteryBetTypeId);
        }

        [Fact]
        public void GetLotteryBetTypeSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var lotteryBetTypeId = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"LotteryBetType?betTypeId={lotteryBetTypeId}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetLotteryBetTypeReturnsValueGotFromService()
        {
            // Arrange.
            var lotteryBetTypeId = _fixture.Create<string>();
            var lotteryBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, lotteryBetTypeName } });

            // Act.
            var actualLotteryBetTypeName = _remotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(lotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void GetLotteryBetTypeWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetTypeId = _fixture.Create<string>();
            var defaultLotteryBetTypeName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetTypeName = _remotingRefService.GetLotteryBetType(
                lotteryBetTypeId,
                TestingLanguageCode,
                defaultLotteryBetTypeName);

            // Assert.
            Assert.Equal(defaultLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void GetLotteryBetTypeWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetTypeId = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetTypeName = _remotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLotteryBetTypeName);
        }
    }
}