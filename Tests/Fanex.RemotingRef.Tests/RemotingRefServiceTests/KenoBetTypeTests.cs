namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RemotingRefServiceTestBase
    {
        private readonly string _kenoBetTypeId;

        public KenoBetTypeTests()
        {
            _kenoBetTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"KenoBetType?kenoBetTypeId={_kenoBetTypeId}");
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
            RemotingRefService.GetKenoBetType(_kenoBetTypeId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var kenoBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(kenoBetTypeName));

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(_kenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(kenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultKenoBetTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                _kenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableKenoBetTypeId(string unacceptableKenoBetTypeId)
        {
            // Arrange.
            var defaultKenoBetTypeName = Fixture.Create<string>();

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                unacceptableKenoBetTypeId,
                TestingLanguageCode,
                defaultKenoBetTypeName);

            // Assert.
            Assert.Equal(defaultKenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableKenoBetTypeId = string.Empty;

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(unacceptableKenoBetTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }
    }
}