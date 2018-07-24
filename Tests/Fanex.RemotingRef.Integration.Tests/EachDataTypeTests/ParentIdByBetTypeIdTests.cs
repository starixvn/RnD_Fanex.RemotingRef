namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class ParentIdByBetTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid betTypeId | GET: expected parent bet type Id")]
        public void GetParentIdWithValidBetTypeId()
        {
            // Arrange.
            var betTypeId = "178";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            var expectedParentBetTypeId = "18";

            Assert.Equal(expectedParentBetTypeId, actualParentBetTypeId);
        }

        [Fact]
        [Description("GIVE: betTypeId has parentTypeId is itself | GET: passed betTypeId")]
        public void GetGivenBetTypeIdWhenParentTypeIdNotExist()
        {
            // Arrange.
            var betTypeId = "1";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            Assert.Equal(betTypeId, actualParentBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid betTypeId | GET: given defaultValue")]
        public void GetDefaultValueWithInvalidBetTypeIdId(string betTypeId)
        {
            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualParentBetTypeId);
        }
    }
}