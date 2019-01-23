namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RemotingRefServiceTestBase
    {
        private readonly string _lotteryBetChoiceId;

        public LotteryBetChoiceTests()
        {
            _lotteryBetChoiceId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"LotteryBetChoice?lotteryBetChoiceId={_lotteryBetChoiceId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<Tuple<string, string>>()));

            // Act.
            RemotingRefService.GetLotteryBetChoice(_lotteryBetChoiceId, TestingLanguageCode, out _);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnLotteryBetChoiceNameGotFromService()
        {
            // Arrange.
            var lotteryBetChoiceName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(new Tuple<string, string>(lotteryBetChoiceName, null)));

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
                TestingLanguageCode,
                out _);

            // Assert.
            Assert.Equal(lotteryBetChoiceName, actualLotteryBetChoiceName);
        }

        [Fact]
        public void SetLotteryBetTypeIdGotFromServiceAsOutput()
        {
            // Arrange.
            var lotteryBetTypeId = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(new Tuple<string, string>(null, lotteryBetTypeId)));

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
                TestingLanguageCode,
                out string actualLotteryBetTypeId);

            // Assert.
            Assert.Equal(lotteryBetTypeId, actualLotteryBetTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultValue = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualLotteryBetChoiceName = RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
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
            var defaultValue = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                _lotteryBetChoiceId,
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