namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RemotingRefServiceTestBase
    {
        private readonly string _oddsTypeId;

        public OddsTypeNameTests()
        {
            _oddsTypeId = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"OddsTypeName?oddsTypeId={_oddsTypeId}");
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
            RemotingRefService.GetOddsTypeName(_oddsTypeId, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var oddsTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(oddsTypeName));

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(_oddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Equal(oddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultOddsTypeName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                _oddsTypeId,
                TestingLanguageCode,
                defaultOddsTypeName);

            // Assert.
            Assert.Equal(defaultOddsTypeName, actualOddsTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void ReturnDefaultValueForUnacceptableOddsTypeId(string unacceptableOddsTypeId)
        {
            // Arrange.
            var defaultOddsTypeName = Fixture.Create<string>();

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(
                unacceptableOddsTypeId,
                TestingLanguageCode,
                defaultOddsTypeName);

            // Assert.
            Assert.Equal(defaultOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableOddsTypeId = string.Empty;

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(unacceptableOddsTypeId, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualOddsTypeName);
        }
    }
}