namespace Fanex.RemotingRef.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public partial class RemotingRefServiceTests
    {
        [Fact]
        public void GetCountryNameSendsRequestWithCorrectParameters()
        {
            // Arrange.
            var countryCode = _fixture.Create<string>();

            // Act.
            _remotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"CountryName?countryCode={countryCode}");

            _httpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void GetCountryNameReturnsValueGotFromService()
        {
            // Arrange.
            var countryCode = _fixture.Create<string>();
            var countryName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { { TestingLanguageCode, countryName } });

            // Act.
            var actualCountryName = _remotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(countryName, actualCountryName);
        }

        [Fact]
        public void GetCountryNameWithDefaultValuePassedReturnsDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var countryCode = _fixture.Create<string>();
            var defaultCountryName = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = _remotingRefService.GetCountryName(countryCode, TestingLanguageCode, defaultCountryName);

            // Assert.
            Assert.Equal(defaultCountryName, actualCountryName);
        }

        [Fact]
        public void GetCountryNameWithoutDefaultValuePassedReturnsEmptyStringIfServiceReturnsNull()
        {
            // Arrange.
            var countryCode = _fixture.Create<string>();

            _serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = _remotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCountryName);
        }
    }
}