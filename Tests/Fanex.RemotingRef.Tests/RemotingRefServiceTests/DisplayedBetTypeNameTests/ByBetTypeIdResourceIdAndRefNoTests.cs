namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.DisplayedBetTypeNameTests
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
                $"DisplayedBetTypeName?betTypeId={_betTypeId}&resourceId={_resourceId}&refNo={_refNo}");
            HttpClient
                .GetAsync(Arg.Is(ExpectedRequestRelativePath))
                .Returns(Response);
        }

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(Fixture.Create<string>()));

            // Act.
            RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var defaultDisplayedBetTypeName = Fixture.Create<string>();
            var displayedBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(displayedBetTypeName));

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

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
                _resourceId,
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
        public void ByUnacceptableBetTypeIdResourceIdAndRefNo_ReturnDefaultValue(string unacceptableBetTypeId)
        {
            // Arrange.
            var defaultDisplayedBetTypeName = Fixture.Create<string>();

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unacceptableBetTypeId,
                _resourceId,
                _refNo,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }
    }
}