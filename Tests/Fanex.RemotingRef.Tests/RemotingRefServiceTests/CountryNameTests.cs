namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RemotingRefServiceTestBase
    {
        private readonly string _countryCode;

        public CountryNameTests()
            => _countryCode = Fixture.Create<string>();

        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Act.
            RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"CountryName?countryCode={_countryCode}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var countryName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = countryName });

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(countryName, actualCountryName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var defaultCountryName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode, defaultCountryName);

            // Assert.
            Assert.Equal(defaultCountryName, actualCountryName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(_countryCode, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCountryName);
        }
    }
}