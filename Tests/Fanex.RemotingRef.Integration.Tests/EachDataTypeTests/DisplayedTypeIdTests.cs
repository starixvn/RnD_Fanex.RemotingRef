namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class DisplayedTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetDisplayedTypeIdByValidBetTypeId()
        {
            // Arrange.
            var validBetTypeId = "303";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(validBetTypeId);

            // Assert.
            var expectedDisplayedBetTypeId = "7";

            Assert.Equal(expectedDisplayedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenItselfDoesNotExist()
        {
            // Arrange.
            var unexistingBetTypeId = "-1";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(unexistingBetTypeId);

            // Assert.
            Assert.Equal(unexistingBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenDisplayedTypeIdDoesNotExist()
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