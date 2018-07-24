namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class DisplayedTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid betTypeId | GET: expected display bet type Id")]
        public void GetDisplayedTypeIdWithValidTeamId()
        {
            // Arrange.
            var betTypeId = "303";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            var expectedBetTypeId = "7";

            Assert.Equal(expectedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        [Description("GIVE: betTypeId has displayedTypeId is itself | GET: passed betTypeId")]
        public void GetGivenBetTypeIdWhenDisplayedTypeIdNotExist()
        {
            // Arrange.
            var betTypeId = "1";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            Assert.Equal(betTypeId, actualDisplayedBetTypeId);
        }
    }
}