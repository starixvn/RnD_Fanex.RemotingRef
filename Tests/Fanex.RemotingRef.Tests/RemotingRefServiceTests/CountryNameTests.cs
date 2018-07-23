namespace Fanex.RemotingRef.Tests.RemotingRefServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class CountryNameTests : RemotingRefServiceTestBase
    {
        [Fact]
        public void SendRequestWithCorrectParameters()
        {
            // Arrange.
            var countryCode = Fixture.Create<string>();

            // Act.
            RemotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            var expectedUrl = string.Format(RequestUrlPattern, $"CountryName?countryCode={countryCode}");

            HttpClient.Received().GetAsync(Arg.Is(expectedUrl));
        }

        [Fact]
        public void ReturnNonNullValueGotFromService()
        {
            // Arrange.
            var countryCode = Fixture.Create<string>();
            var countryName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, string> { [TestingLanguageCode] = countryName });

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            Assert.Equal(countryName, actualCountryName);
        }

        [Fact]
        public void ReturnProvidedDefaultValueIfServiceReturnsNull()
        {
            // Arrange.
            var countryCode = Fixture.Create<string>();
            var defaultCountryName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguageCode, defaultCountryName);

            // Assert.
            Assert.Equal(defaultCountryName, actualCountryName);
        }

        [Fact]
        public void ReturnEmptyStringIfServiceReturnsNullAndNoDefaultValueProvided()
        {
            // Arrange.
            var countryCode = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguageCode);

            // Assert.
            Assert.Empty(actualCountryName);
        }
    }
}