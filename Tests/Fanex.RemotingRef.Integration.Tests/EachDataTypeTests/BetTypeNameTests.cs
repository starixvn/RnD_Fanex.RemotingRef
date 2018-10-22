namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class BetTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing betTypeId | GET: expected bet type name")]
        public void GetBetTypeNameByExistingBetTypeId()
        {
            // Arrange.
            var existingBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(existingBetTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting betTypeId | GET: empty string")]
        public void GetEmptyStringByUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(unexistingBetTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo > one hundred billions | GET: bet type name containing '(Via Lucky)'")]
        public void GetViaLuckyBetTypeNameWithOverOneHundredBillionRefNo()
        {
            // Arrange.
            var existingBetTypeId = "1";
            var satisfiedRefNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                satisfiedRefNo);

            // Assert.
            var expectedBetTypeName = "Handicap (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one hundred billions | GET: bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            var existingBetTypeId = "1";
            var unsatisfiedRefNo = 99_000_000_000L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                unsatisfiedRefNo);

            // Assert.
            var expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-2";
            var satisfiedRefNo = 100_000_000_002L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unexistingBetTypeId,
                TestingLanguage,
                satisfiedRefNo);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-3";
            var satisfiedRefNo = 100_000_000_003L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unexistingBetTypeId,
                TestingLanguage,
                satisfiedRefNo,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: nonnull or non-empty resourceId | GET: bet type with any X, Y, or Z replaced by resourceId")]
        public void GetBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var existingBetTypeId = "606";
            var validResourceId = "10";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId);

            // Assert.
            var expectedBetTypeName = "1H Race To 10 Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original bet type with any X, Y, or Z not replaced")]
        public void GetOriginalNameWithNullOrEmptyResourceId(string invalidResourceId)
        {
            // Arrange.
            var existingBetTypeId = "606";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                invalidResourceId);

            // Assert.
            var expectedBetTypeName = "1H Race To X Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or empty) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithNonEmptyResourceIdButUnexistingBetTypeId()
        {
            // Arrange.
            var unexistingBetTypeId = "-4";
            var validResourceId = "10";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unexistingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (refNo > one hundred billions) && (nonnull or non-empty resourceId) | " +
                     "GET: bet type with any X, Y, or Z replaced by resourceId, then suffix it with '(Via Lucky)'")]
        public void GetViaLuckyBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var existingBetTypeId = "606";
            var validResourceId = "10";
            var satisfiedRefNo = 100_000_000_004L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId,
                satisfiedRefNo);

            // Assert.
            var expectedBetTypeName = "1H Race To 10 Points (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
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
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                unexistingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId,
                satisfiedRefNo);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }
    }
}