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
            const string existingBetTypeId = "406";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(existingBetTypeId, TestingLanguage);

            // Assert.
            const string expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: unexisting betTypeId | GET: empty string")]
        public void GetEmptyStringByUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-1";

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
            const string existingBetTypeId = "406";
            const long satisfiedRefNo = 100_000_000_001;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                satisfiedRefNo,
                TestingLanguage);

            // Assert.
            const string expectedDisplayedBetTypeName = "Exact Total Goals (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one hundred billions | GET: displayed bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalDisplayedNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            const string existingBetTypeId = "406";
            const long unsatisfiedRefNo = 99_000_000_000;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                unsatisfiedRefNo,
                TestingLanguage);

            // Assert.
            const string expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (refNo > one hundred billions) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithOverOneHundredBillionRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-2";
            const long satisfiedRefNo = 100_00_000_002;

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
            const string unexistingBetTypeId = "-3";
            const long satisfiedRefNo = 100_000_000_03;

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
            const string existingBetTypeId = "1311";
            const string validResourceId = "10";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                validResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedDisplayedBetTypeName = "Set 10 Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original displayed bet type name with any X, Y, or Z not replaced")]
        public void GetOriginalDisplayedNameWithNullOrEmptyResourceId(string invalidResourceId)
        {
            // Arrange.
            const string existingBetTypeId = "1311";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                invalidResourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedDisplayedBetTypeName = "Set X Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or not empty) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithNonEmptyResourceIdButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-4";
            const string validResourceId = "10";

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
            const string existingBetTypeId = "1311";
            const string validResourceId = "10";
            const long satisfiedRefNo = 100_000_000_004;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                existingBetTypeId,
                validResourceId,
                satisfiedRefNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedDisplayedBetTypeName = "Set 10 Winner (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (unexisting betTypeId) && (resourceId is not null or empty) && (refNo > one hundred billions) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithSatisfiedResourceIdAndRefNoButUnexistingBetTypeId()
        {
            // Arrange.
            const string unexistingBetTypeId = "-5";
            const string validResourceId = "10";
            const long satisfiedRefNo = 1_000_000_005;

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