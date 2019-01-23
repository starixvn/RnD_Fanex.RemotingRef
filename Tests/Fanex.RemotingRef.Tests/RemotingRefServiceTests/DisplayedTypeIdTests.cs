namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;

        public DisplayedTypeIdTests()
        {
            _betTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"DisplayedTypeId?betTypeId={_betTypeId}");
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
            RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var displayTypeId = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(displayTypeId));

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(displayTypeId, actualDisplayTypeId);
        }

        [Fact]
        public void ReturnProvidedBetTypeIdWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(_betTypeId);

            // Assert.
            Assert.Equal(_betTypeId, actualDisplayTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnProvidedBetTypeIdWhenItIsUnacceptable(string unacceptableBetTypeId)
        {
            // Act.
            var actualDisplayTypeId = RemotingRefService.GetDisplayedTypeId(unacceptableBetTypeId);

            // Assert.
            Assert.Equal(unacceptableBetTypeId, actualDisplayTypeId);
        }
    }
}