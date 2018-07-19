namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RemotingRefServiceBaseTests
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLotteryBetChoice(lotteryBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"LotteryBetChoice?betChoiceId={lotteryBetChoiceId}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();
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
                lotteryBetChoiceId,
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
            var lotteryBetChoiceId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
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
            var lotteryBetChoiceId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualTeamName = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Empty(actualTeamName);
            Assert.Empty(actualLotteryBetTypeId);
        }
    }
}