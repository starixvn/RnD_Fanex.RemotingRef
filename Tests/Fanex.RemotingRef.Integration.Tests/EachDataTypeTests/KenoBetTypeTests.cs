namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetTypeTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing kenoBetTypeId | GET: expected Keno bet type name")]
        public void GetKenoBetTypeNameByExistingKenoBetTypeId()
        {
            // Arrange.
            var existingKenoBetTypeId = "2";

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(existingKenoBetTypeId, TestingLanguage);

            // Assert.
            var expectedKenoBetTypeName = "Odd/Even";

            Assert.Equal(expectedKenoBetTypeName, actualKenoBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting kenoBetTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringByUnexistingKenoBetTypeId()
        {
            // Arrange.
            var unexistingKenoBetTypeId = "-1";

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(unexistingKenoBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualKenoBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting kenoBetTypeId | GET: given defaultValue")]
        public void TakeBackDefaultValueWithUnexistingKenoBetTypeId()
        {
            // Arrange.
            var unexistingKenoBetTypeId = "-2";

            // Act.
            var actualKenoBetTypeName = RemotingRefService.GetKenoBetType(
                unexistingKenoBetTypeId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualKenoBetTypeName);
        }
    }
}