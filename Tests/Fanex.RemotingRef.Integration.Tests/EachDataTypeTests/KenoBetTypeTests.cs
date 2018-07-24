namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid kenoBetTypeId | GET: expected Keno bet type name")]
        public void GetKenoBetTypeNameWithValidKenoBetTypeId()
        {
            // Arrange.
            var kenoBetTypeId = "2";

            // Act.
            var actualBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "Odd/Even";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid kenoBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithInvalidKenoBetTypeId(string kenoBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid kenoBetTypeId | GET: given defaultValue")]
        public void GetGivenDefaultValueWithInvalidKenoBetTypeId(string kenoBetTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetKenoBetType(
                kenoBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }
    }
}