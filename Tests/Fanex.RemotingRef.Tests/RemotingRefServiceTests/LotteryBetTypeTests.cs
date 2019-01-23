namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RemotingRefServiceTestBase
    {
        private readonly string _lotteryBetTypeId;

        public LotteryBetTypeTests()
        {
            _lotteryBetTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"LotteryBetType?lotteryBetTypeId={_lotteryBetTypeId}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetLotteryBetType(_lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var lotteryBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(lotteryBetTypeName));

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(_lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(lotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultLotteryBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                _lotteryBetTypeId,
                TestingLanguageCode,
                defaultLotteryBetTypeName);

            // Assert.
            Assert.Equal(defaultLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableLotteryBetTypeId(string unacceptableLotteryBetTypeId)
        {
            // Arrange.
            var defaultLotteryBetTypeName = Fixture.Create<string>();

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                unacceptableLotteryBetTypeId,
                TestingLanguageCode,
                defaultLotteryBetTypeName);

            // Assert.
            Assert.Equal(defaultLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableLotteryBetTypeId = string.Empty;

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(unacceptableLotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLotteryBetTypeName);
        }
    }
}