namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.BetTypeNameTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ByBetTypeIdAndRefNoTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly long _refNo;

        public ByBetTypeIdAndRefNoTests()
        {
            _betTypeId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetTypeName?betTypeId={_betTypeId}&refNo={_refNo}");
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
            RemotingRefService.GetBetTypeName(
                _betTypeId,
                TestingLanguageCode,
                _refNo);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var betTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(betTypeName));

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                _betTypeId,
                TestingLanguageCode,
                _refNo);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                _betTypeId,
                TestingLanguageCode,
                _refNo,
                defaultBetTypeName);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Arrange.
            var defaultBetTypeName = Fixture.Create<string>();

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                _refNo,
                defaultBetTypeName);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableBetTypeId = string.Empty;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unacceptableBetTypeId,
                TestingLanguageCode,
                _refNo);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }
    }
}