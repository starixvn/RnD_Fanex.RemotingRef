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
            const string existingBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(existingBetTypeId, TestingLanguage);

            // Assert.
            const string expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting betTypeId | GET: empty string")]
        public void GetEmptyStringByUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-1";

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
            const string existingBetTypeId = "1";
            const long satisfiedRefNo = 100_000_000_001;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                satisfiedRefNo);

            // Assert.
            const string expectedBetTypeName = "Handicap (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one hundred billions | GET: bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            const string existingBetTypeId = "1";
            const long unsatisfiedRefNo = 99_000_000_000;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                unsatisfiedRefNo);

            // Assert.
            const string expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-2";
            const long satisfiedRefNo = 100_000_000_002;

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
            const string unexistingBetTypeId = "-3";
            const long satisfiedRefNo = 100_000_000_003;

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
            const string existingBetTypeId = "606";
            const string validResourceId = "10";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId);

            // Assert.
            const string expectedBetTypeName = "1H Race To 10 Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original bet type with any X, Y, or Z not replaced")]
        public void GetOriginalNameWithNullOrEmptyResourceId(string invalidResourceId)
        {
            // Arrange.
            const string existingBetTypeId = "606";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                invalidResourceId);

            // Assert.
            const string expectedBetTypeName = "1H Race To X Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or empty) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithNonEmptyResourceIdButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-4";
            const string validResourceId = "10";

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
            const string existingBetTypeId = "606";
            const string validResourceId = "10";
            const long satisfiedRefNo = 100_000_000_004;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                existingBetTypeId,
                TestingLanguage,
                DefaultValue,
                validResourceId,
                satisfiedRefNo);

            // Assert.
            const string expectedBetTypeName = "1H Race To 10 Points (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or empty) && (refNo > one hundred billions) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithSatisfiedResourceIdAndRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-5";
            const string validResourceId = "10";
            const long satisfiedRefNo = 100_000_000_005;

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