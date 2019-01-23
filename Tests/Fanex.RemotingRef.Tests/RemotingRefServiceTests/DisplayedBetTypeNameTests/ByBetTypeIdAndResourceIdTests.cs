namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.DisplayedBetTypeNameTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ByBetTypeIdAndResourceIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _resourceId;

        public ByBetTypeIdAndResourceIdTests()
        {
            _betTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(
                RequestRelativePathPattern,
                $"DisplayedBetTypeName?betTypeId={_betTypeId}&resourceId={_resourceId}");
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
                _resourceId,
                TestingLanguageCode,
                defaultDisplayedBetTypeName);

            // Assert.
            Assert.Equal(defaultDisplayedBetTypeName, actualDisplayedBetTypeName);
        }
    }
}