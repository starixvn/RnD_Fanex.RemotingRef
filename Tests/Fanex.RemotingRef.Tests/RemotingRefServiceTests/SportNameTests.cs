namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class SportNameTests : RemotingRefServiceTestBase
    {
        private readonly string _sportTypeId;

        public SportNameTests()
        {
            _sportTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"SportName?sportTypeId={_sportTypeId}");
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
            RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var sportTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(sportTypeName));

            // Act.
            var actualSportName = RemotingRefService.GetSportName(_sportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(sportTypeName, actualSportName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultSportTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualSportName = RemotingRefService.GetSportName(
                _sportTypeId,
                TestingLanguageCode,
                defaultSportTypeName);

            // Assert.
            Assert.Equal(defaultSportTypeName, actualSportName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableSportTypeId(string unacceptableSportTypeId)
        {
            // Arrange.
            var defaultSportTypeName = Fixture.Create<string>();

            // Act.
            var actualSportName = RemotingRefService.GetSportName(
                unacceptableSportTypeId,
                TestingLanguageCode,
                defaultSportTypeName);

            // Assert.
            Assert.Equal(defaultSportTypeName, actualSportName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableSportTypeId = string.Empty;

            // Act.
            var actualSportName = RemotingRefService.GetSportName(unacceptableSportTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualSportName);
        }
    }
}