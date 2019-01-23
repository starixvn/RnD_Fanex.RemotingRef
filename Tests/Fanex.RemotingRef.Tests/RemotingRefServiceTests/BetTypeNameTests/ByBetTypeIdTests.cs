namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests.BetTypeNameTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ByBetTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;

        public ByBetTypeIdTests()
        {
            _betTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"BetTypeName?betTypeId={_betTypeId}");
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
            RemotingRefService.GetBetTypeName(_betTypeId, TestingLanguageCode);

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
            var actualBetTypeName = RemotingRefService.GetBetTypeName(_betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(betTypeName, actualBetTypeName);
        }

        [Fact]
        public void ReturnEmptyStringWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(_betTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnEmptyStringForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(unacceptableBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }
    }
}