namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class DisplayedTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetDisplayedTypeIdByValidBetTypeId()
        {
            // Arrange.
            var betTypeId = "303";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            var expectedDisplayedBetTypeId = "7";

            Assert.Equal(expectedDisplayedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenDisplayedTypeIdNotExist()
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