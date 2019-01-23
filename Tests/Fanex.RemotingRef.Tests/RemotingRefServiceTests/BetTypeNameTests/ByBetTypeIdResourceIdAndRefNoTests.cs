namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.BetTypeNameTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ByBetTypeIdResourceIdAndRefNoTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _resourceId;
        private readonly long _refNo;

        public ByBetTypeIdResourceIdAndRefNoTests()
        {
            _betTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"BetTypeName?betTypeId={_betTypeId}&resourceId={_resourceId}&refNo={_refNo}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var defaultBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetBetTypeName(
                _betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                _resourceId,
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
            var defaultBetTypeName = Fixture.Create<string>();
            var betTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(betTypeName));

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                _betTypeId,
                TestingLanguageCode,
                defaultBetTypeName,
                _resourceId,
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
                defaultBetTypeName,
                _resourceId,
                _refNo);

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
                defaultBetTypeName,
                _resourceId,
                _refNo);

            // Assert.
            Assert.Equal(defaultBetTypeName, actualBetTypeName);
        }
    }
}