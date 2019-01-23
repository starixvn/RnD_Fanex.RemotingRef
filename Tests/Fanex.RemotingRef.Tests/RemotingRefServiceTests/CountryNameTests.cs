namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RemotingRefServiceTestBase
    {
        private readonly string _countryCode;

        public CountryNameTests()
        {
            _countryCode = Fixture.Create<string>();

            ExpectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"CountryName?countryCode={_countryCode}");
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
            RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode);

            // Assert.
            HttpClient
                .Received()
                .GetAsync(Arg.Is(ExpectedRequestRelativePath));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var countryName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet(countryName));

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(countryName, actualCountryName);
        }

        [Fact]
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var defaultCountryName = Fixture.Create<string>();

            Response.Content = FakeResponseContent(SerializeTranslationSet<object>(null));

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(
                _countryCode,
                TestingLanguageCode,
                defaultCountryName);

            // Assert.
            Assert.Equal(defaultCountryName, actualCountryName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnDefaultValueForNullOrEmptyCountryCode(string unacceptableCountryCode)
        {
            // Arrange.
            var defaultCountryName = Fixture.Create<string>();

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(
                unacceptableCountryCode,
                TestingLanguageCode,
                defaultCountryName);

            // Assert.
            Assert.Equal(defaultCountryName, actualCountryName);
        }

        [Fact]
        public void TakeEmptyStringAsDefaultValueWhenNotProvided()
        {
            // Arrange.
            var unacceptableCountryCode = string.Empty;

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(unacceptableCountryCode, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCountryName);
        }
    }
}