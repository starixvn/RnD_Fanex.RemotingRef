namespace Fanex.RemotingRef.Integration.Tests
{
    using Xunit;

    public class ParentIdByBetTypeIdTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetParentIdByValidBetTypeId()
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
        public void TakeBackBetTypeIdWhenParentTypeIdNotExist()
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
        [InlineData("0")]
        public void TakeBackDefaultValueWithInvalidBetTypeId(string betTypeId)
        {
            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualParentBetTypeId);
        }
    }
}