namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class CountryNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing countryCode | GET: expected country name")]
        public void GetCountryNameByExistingCountryCode()
        {
            // Arrange.
            var existingCountryCode = "AUS";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(existingCountryCode, TestingLanguage);

            // Assert.
            var expectedCountryName = "Australia";

            Assert.Equal(expectedCountryName, actualCountryName);
        }

        [Fact]
        [Description("GIVE: (unexisting countryCode) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingCountryCode()
        {
            // Arrange.
            var unexistingCountryCode = "**CK";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(unexistingCountryCode, TestingLanguage);

            // Assert.
            Assert.Empty(actualCountryName);
        }

        [Fact]
        [Description("GIVE: unexisting countryCode | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingCountryCode()
        {
            // Arrange.
            var unexistingCountryCode = "SH**";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(
                unexistingCountryCode,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualCountryName);
        }
    }
}