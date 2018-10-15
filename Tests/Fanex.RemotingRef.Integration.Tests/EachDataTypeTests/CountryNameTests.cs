namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CountryNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetCountryNameByValidCountryCode()
        {
            // Arrange.
            var countryCode = "AUS";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguage);

            // Assert.
            var expectedCountryName = "Australia";

            Assert.Equal(expectedCountryName, actualCountryName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("**CK")]
        [Description("GIVE: (invalid countryCode) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidCountryCode(string countryCode)
        {
            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguage);

            // Assert.
            Assert.Empty(actualCountryName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("**CK")]
        [Description("GIVE: invalid countryCode | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidCountryCode(string countryCode)
        {
            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(
                countryCode,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualCountryName);
        }
    }
}