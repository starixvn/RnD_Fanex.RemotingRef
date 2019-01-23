namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.DisplayedBetTypeNameTests
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
                $"DisplayedBetTypeName?betTypeId={_betTypeId}&refNo={_refNo}");
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
            RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                _refNo,
                TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var displayedBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(displayedBetTypeName));

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                _refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Equal(displayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                _refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Arrange.
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                _refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableBetTypeId = string.Empty;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                _refNo,
                TestingLanguageCode);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }
    }
}