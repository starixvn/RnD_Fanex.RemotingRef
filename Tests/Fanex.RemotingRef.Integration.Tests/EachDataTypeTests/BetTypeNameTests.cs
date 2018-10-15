namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class BetTypeNameTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid betTypeId | GET: expected bet type name")]
        public void GetBetTypeNameWithValidBetTypeId()
        {
            // Arrange.
            var betTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid betTypeId | GET: empty string")]
        public void GetEmptyStringByInvalidBetTypeId(string betTypeId)
        {
            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo > one hundred billions | GET: bet type name containing '(Via Lucky)'")]
        public void GetViaLuckyBetTypeNameWithOverOneHundredBillionRefNo()
        {
            // Arrange.
            var betTypeId = "1";
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo);

            // Assert.
            var expectedBetTypeName = "Handicap (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: refNo <= one hundred billions | GET: bet type name without '(Via Lucky)' suffixed")]
        public void GetOriginalNameWithUnderOneHundredBillionRefNo()
        {
            // Arrange.
            var betTypeId = "1";
            var refNo = 99_000_000_000L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo);

            // Assert.
            var expectedBetTypeName = "Handicap";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (refNo > one hundred billions) && (invalid betTypeId) && (no defaultValue provided) | GET: empty string")]
        public void GetEmptyStringWithSatisfiedRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo);

            // Assert.
            Assert.Empty(actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (refNo > one hundred billions) && (invalid betTypeId) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithSatisfiedRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo,
                DefaultValue);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: nonnull or non-empty resourceId | GET: bet type with any X, Y, or Z replaced by resourceId")]
        public void GetBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var betTypeId = "606";
            var resourceId = "10";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId);

            // Assert.
            var expectedBetTypeName = "1H Race To 10 Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Description("GIVE: null or empty resourceId | GET: original bet type with any X, Y, or Z not replaced")]
        public void GetOriginalNameWithNullOrEmptyResourceId(string resourceId)
        {
            // Arrange.
            var betTypeId = "606";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId);

            // Assert.
            var expectedBetTypeName = "1H Race To X Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (resourceId is not null or empty) && (invalid betTypeId) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithNonEmptyResourceIdButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var resourceId = "10";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }

        [Fact]
        [Description("GIVE: (refNo > one hundred billions) && (nonnull or non-empty resourceId) | " +
                     "GET: bet type with any X, Y, or Z replaced by resourceId, then suffix it with '(Via Lucky)'")]
        public void GetViaLuckyBetTypeNameWithPlaceholdersReplacedByResourceId()
        {
            // Arrange.
            var betTypeId = "606";
            var resourceId = "10";
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId,
                refNo);

            // Assert.
            var expectedBetTypeName = "1H Race To 10 Points (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (resourceId is not null or empty) && (refNo > one hundred billions) && (invalid betTypeId) | GET: given defaultValue")]
        public void TakeBackDefaultValueWithSatisfiedResourceIdAndRefNoButInvalidBetTypeId(string betTypeId)
        {
            // Arrange.
            var resourceId = "10";
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId,
                refNo);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeName);
        }
    }
}