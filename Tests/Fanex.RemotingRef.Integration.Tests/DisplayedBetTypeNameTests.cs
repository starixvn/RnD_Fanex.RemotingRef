namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class DisplayedBetTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid betTypeId | GET: expected displayed bet type name")]
        public void GetDisplayedBetTypeNameWithValidBetTypeId()
        {
            // Arrange.
            var betTypeId = "406";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid betTypeId | GET: empty string")]
        public void GetEmptyStringWithInvalidBetTypeId(string betTypeId)
        {
            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo > one billions | GET: displayed bet type name containing '(Via Lucky)'")]
        public void GetDisplayedViaLuckyBetTypeNameWithOverOneHundredBillionRefNo()
        {
            // Arrange.
            var betTypeId = "406";
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one billions | GET: displayed bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalDisplayedNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            var betTypeId = "406";
            var refNo = 99_000_000_000L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Total Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (refNo > one billions) && (invalid betTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithSatisfiedRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage);

            // Assert.
            Assert.Empty(actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (refNo > one billions) && (invalid betTypeId) | GET: given defaultValue")]
        public void GetGivenDefaultValueWithSatisfiedRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: nonnull or non-empty resourceId | GET: displayed bet type with any X, Y, or Z replaced by resourceId")]
        public void GetDisplayedBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var betTypeId = "1311";
            var resourceId = "10";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 10 Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original displayed bet type with any X, Y, or Z not replaced")]
        public void GetOriginalDisplayedNameWithNullOrEmptyResourceId(string resourceId)
        {
            // Arrange.
            var betTypeId = "1311";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set x Winner";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (resourceId is not null or empty) && (invalid betTypeId) | GET: given defaultValue")]
        public void GetGivenDefaultValueWithNonEmptyResourceIdButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var resourceId = "10";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("GIVE: (refNo > one billions) && (nonnull or non-empty resourceId) | " +
                     "GET: displayed bet type with any X, Y, or Z replaced by resourceId, then suffix it with '(Via Lucky)'")]
        public void GetDisplayedViaLuckyBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var betTypeId = "1311";
            var resourceId = "10";
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 10 Winner (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (resourceId is not null or empty) && (refNo > one billions) && (invalid betTypeId) | GET: given defaultValue")]
        public void GetGivenDefaultValueWithSatisfiedResourceIdAndRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var resourceId = "10";
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplyedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualDisplyedBetTypeName);
        }
    }
}