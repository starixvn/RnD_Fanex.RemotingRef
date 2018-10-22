namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var lotteryBetTypeId = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            var expectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"LotteryBetType?lotteryBetTypeId={lotteryBetTypeId}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var lotteryBetTypeId = Fixture.Create<string>();
            var lotteryBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = lotteryBetTypeName });

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(lotteryBetTypeName, actualLotteryBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var lotteryBetTypeId = Fixture.Create<string>();
            var defaultLotteryBetTypeName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualLotteryBetTypeName = RemotingRefService.GetLotteryBetType(
                lotteryBetTypeId,
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