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
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLotteryBetChoice(lotteryBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"LotteryBetChoice?lotteryBetChoiceId={lotteryBetChoiceId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnLotteryBetChoiceNameGotFromService()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();
            var lotteryBetChoiceName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(lotteryBetChoiceName, null)
                });

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                out _);

            // Assert.
            Assert.Equal(lotteryBetChoiceName, actualLotteryBetChoiceName);
        }

        [Fact]
        public void SetLotteryBetTypeIdGotFromServiceAsOutput()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();
            var lotteryBetTypeId = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>>
                {
                    [TestingLanguageCode] = new Tuple<string, string>(null, lotteryBetTypeId)
                });

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(lotteryBetTypeId, actualLotteryBetTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
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
                out _);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetChoiceName);
        }

        [Fact]
        public void SetDefaultValueAsOutputWhenServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetChoiceId = Fixture.Create<string>();
            var defaultValue = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableLotteryBetChoiceId(string unacceptableLotteryBetChoiceId)
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                unacceptableLotteryBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out _);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetChoiceName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void SetDefaultValueAsOutputForUnacceptableLotteryBetChoiceId(string unacceptableLotteryBetChoiceId)
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                unacceptableLotteryBetChoiceId,
                TestingLanguageCode,
                defaultValue,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(defaultValue, actualLotteryBetTypeId);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableLotteryBetChoiceId = string.Empty;

            // Act.
            var actualLotteryBetChoice = RemotingRefService.GetLotteryBetChoice(
                unacceptableLotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Empty(actualLotteryBetChoice);
            Assert.Empty(actualLotteryBetTypeId);
        }
    }
}