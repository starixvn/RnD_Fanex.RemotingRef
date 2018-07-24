namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: valid kenoBetChoiceId | GET: expected Keno bet choice")]
        public void GetKenoBetChoiceWithValidBetChoiceId()
        {
            // Arrange.
            var kenoBetChoiceId = "2";

            // Act.
            var actualBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedBetChoice = "Small";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        [Description("GIVE: valid kenoBetChoiceId | GET: expected Keno bet type Id")]
        public void GetKenoBetTypeIdWithValidBetChoiceId()
        {
            // Arrange.
            var kenoBetChoiceId = "2";

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            var expectedBetTypeId = "1";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet choice")]
        public void GetEmptyStringAsKenoBetChoiceWithInvalidBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            var actualBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            Assert.Empty(actualBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: (invalid kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet type Id")]
        public void GetEmptyStringAsKenoBetTypeIdWithInvalidBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            Assert.Empty(actualBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid kenoBetChoiceId | GET: given defaultValue as Keno bet choice")]
        public void GetGivenDefaultAsBetChoiceValueWithInvalidBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            var actualBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out _);

            // Assert.
            Assert.Equal(DefaultValue, actualBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [Description("GIVE: invalid kenoBetChoiceId | GET: given defaultValue as Keno bet type Id")]
        public void GetGivenDefaultValueAsBetTypeIdWithInvalidBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out string actualBetTypeId);

            // Assert.
            Assert.Equal(DefaultValue, actualBetTypeId);
        }
    }
}