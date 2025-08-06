namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ParentIdByBetTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetParentIdByValidBetTypeId()
        {
            // Arrange.
            const string validBetTypeId = "178";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(validBetTypeId, DefaultValue);

            // Assert.
            const string expectedParentBetTypeId = "18";

            Assert.Equal(expectedParentBetTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void TakeBackBetTypeIdWhenParentTypeIdDoesNotExist()
        {
            // Arrange.
            const string betTypeId = "1";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            Assert.Equal(betTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void TakeBackDefaultValueWithUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-1";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(unexistingBetTypeId, DefaultValue);

            // Assert.
            Assert.Equal("0", actualParentBetTypeId);
        }
    }
}