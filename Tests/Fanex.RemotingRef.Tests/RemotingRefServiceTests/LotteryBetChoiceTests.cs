namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _lotteryBetChoiceId;

        public LotteryBetChoiceTests()
            => _lotteryBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetLotteryBetChoice(_lotteryBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"LotteryBetChoice?lotteryBetChoiceId={_lotteryBetChoiceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var lotteryBetChoiceName = Fixture.Create<string>();
            var lotteryBetTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(lotteryBetChoiceName, lotteryBetTypeId)
                });

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(lotteryBetChoiceName, actualLotteryBetChoiceName);
            Assert.Equal(lotteryBetTypeId, actualLotteryBetTypeId);
        }

        [Fact]
        public void SetProvidedDefaultValueAsOutputAndReturnIfServiceReturnsNull()
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetChoiceName);
            Assert.Equal(defaultValue, actualLotteryBetTypeId);
        }

        [Fact]
        public void SetEmptyStringAsOutputAndReturnIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Empty(actualTeamName);
            Assert.Empty(actualLotteryBetTypeId);
        }
    }
}