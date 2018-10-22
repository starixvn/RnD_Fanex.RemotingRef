namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class DisplayedBetTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing betTypeId | GET: expected displayed bet type name")]
        public void GetDisplayedBetTypeNameByExistingBetTypeId()
        {
            // Arrange.
            var existingBetTypeId = "406";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(existingBetTypeId, TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting betTypeId | GET: empty string")]
        public void GetEmptyStringByUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-1";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(unexistingBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo > one hundred billions | GET: displayed bet type name containing '(Via Lucky)'")]
        public void GetDisplayedViaLuckyBetTypeNameWithOverOneHundredBillionRefNo()
        {
            // Arrange.
            var existingBetTypeId = "406";
            var satisfiedRefNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                satisfiedRefNo,
                TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one hundred billions | GET: displayed bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalDisplayedNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            var existingBetTypeId = "406";
            var unsatisfiedRefNo = 99_000_000_000L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                unsatisfiedRefNo,
                TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-2";
            var satisfiedRefNo = 100_000_000_002L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unexistingBetTypeId,
                satisfiedRefNo,
                TestingLanguage);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-3";
            var satisfiedRefNo = 100_000_000_003L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unexistingBetTypeId,
                satisfiedRefNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: nonnull or non-empty resourceId | GET: displayed bet type name with any X, Y, or Z replaced by resourceId")]
        public void GetDisplayedBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var existingBetTypeId = "1311";
            var validResourceId = "10";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 10 Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original displayed bet type name with any X, Y, or Z not replaced")]
        public void GetOriginalDisplayedNameWithNullOrEmptyResourceId(string invalidResourceId)
        {
            // Arrange.
            var existingBetTypeId = "1311";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                invalidResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set x Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or not empty) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithNonEmptyResourceIdButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-4";
            var validResourceId = "10";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unexistingBetTypeId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (refNo > one hundred billions) && (nonnull or non-empty resourceId) | " +
                     "GET: displayed bet type with any X, Y, or Z replaced by resourceId, then suffix it with '(Via Lucky)'")]
        public void GetDisplayedViaLuckyBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var existingBetTypeId = "1311";
            var validResourceId = "10";
            var satisfiedRefNo = 100_000_000_004L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                validResourceId,
                satisfiedRefNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 10 Winner (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or empty) && (refNo > one hundred billions) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithSatisfiedResourceIdAndRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-5";
            var validResourceId = "10";
            var satisfiedRefNo = 100_000_000_005L;

            // Act.
            var actualDisplyedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                unexistingBetTypeId,
                validResourceId,
                satisfiedRefNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplyedBetTypeName);
        }
    }
}