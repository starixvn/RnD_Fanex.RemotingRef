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
            var expectedRequestRelativePath = string.Format(RequestRelativePathPattern, $"CountryName?countryCode={countryCode}");

            HttpClient
                .Received()
                .GetAsync(Arg.Is(expectedRequestRelativePath));
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
        public void ReturnDefaultValueWhenServiceReturnsNull()
        {
            // Arrange.
            var countryCode = Fixture.Create<string>();
            var defaultCountryName = Fixture.Create<string>();

            Serializer
                .Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
                .Returns((Dictionary<string, string>)null);

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(
                countryCode,
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