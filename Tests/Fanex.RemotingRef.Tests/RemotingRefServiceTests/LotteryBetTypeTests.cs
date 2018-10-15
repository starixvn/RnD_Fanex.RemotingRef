namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RemotingRefServiceTestBase
    {
        private readonly string _lotteryBetTypeId;

        public LotteryBetTypeTests()
            => _lotteryBetTypeId = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetLotteryBetType(_lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            string expectedUrl = string.Format(RequestUrlPattern, $"LotteryBetType?lotteryBetTypeId={_lotteryBetTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var lotteryBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = lotteryBetTypeName });

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(_lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(lotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultLotteryBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                _lotteryBetTypeId,
                TestingLanguageCode,
                defaultLotteryBetTypeName);

            // Assert.
            Assert.Equal(defaultLotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(_lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualLotteryBetTypeName);
        }
    }
}