namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class DisplayedTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetDisplayedTypeIdByValidBetTypeId()
        {
            // Arrange.
            const string validBetTypeId = "303";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(validBetTypeId);

            // Assert.
            const string expectedDisplayedBetTypeId = "7";

            Assert.Equal(expectedDisplayedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenItselfDoesNotExist()
        {
            // Arrange.
            const string unexistingBetTypeId = "-1";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(unexistingBetTypeId);

            // Assert.
            Assert.Equal(unexistingBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenDisplayedTypeIdDoesNotExist()
        {
            // Arrange.
            const string betTypeId = "1";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            Assert.Equal(betTypeId, actualDisplayedBetTypeId);
        }
    }
}