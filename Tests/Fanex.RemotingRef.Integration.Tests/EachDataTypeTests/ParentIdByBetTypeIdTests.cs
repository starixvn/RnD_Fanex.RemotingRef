namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ParentIdByBetTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetParentIdByValidBetTypeId()
        {
            // Arrange.
            var validBetTypeId = "178";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(validBetTypeId, DefaultValue);

            // Assert.
            var expectedParentBetTypeId = "18";

            Assert.Equal(expectedParentBetTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenParentTypeIdDoesNotExist()
        {
            // Arrange.
            var betTypeId = "1";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            Assert.Equal(betTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void TakeBackDefaultValueWithUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-1";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(unexistingBetTypeId, DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualParentBetTypeId);
        }
    }
}