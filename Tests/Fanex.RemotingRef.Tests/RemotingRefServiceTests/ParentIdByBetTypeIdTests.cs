namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RemotingRefServiceTestBase
    {
        private readonly string _betTypeId;
        private readonly string _defaultParentTypeId;

        public ParentIdByBetTypeIdTests()
        {
            _betTypeId = Fixture.Create<string>();
            _defaultParentTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"ParentIdByBetTypeId?betTypeId={_betTypeId}");
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
            RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var parentTypeId = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(parentTypeId));

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(parentTypeId, actualParentTypeId);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(_betTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(_defaultParentTypeId, actualParentTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableBetTypeId(string unacceptableBetTypeId)
        {
            // Act.
            var actualParentTypeId = RemotingRefService.GetParentIdByBetTypeId(unacceptableBetTypeId, _defaultParentTypeId);

            // Assert.
            Assert.Equal(_defaultParentTypeId, actualParentTypeId);
        }
    }
}