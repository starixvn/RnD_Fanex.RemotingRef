namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetKenoBetTypeNameByValidKenoBetTypeId()
        {
            // Arrange.
            var kenoBetTypeId = "2";

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            var expectedKenoBetTypeName = "Odd/Even";

            Assert.Equal(expectedKenoBetTypeName, actualKenoBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid kenoBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByInvalidKenoBetTypeId(string kenoBetTypeId)
        {
            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid kenoBetTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithInvalidKenoBetTypeId(string kenoBetTypeId)
        {
            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                kenoBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualKenoBetTypeName);
        }
    }
}