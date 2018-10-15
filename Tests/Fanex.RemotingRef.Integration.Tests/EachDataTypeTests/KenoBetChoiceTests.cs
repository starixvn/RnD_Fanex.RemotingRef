namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetKenoBetChoiceByValidKenoBetChoiceId()
        {
            // Arrange.
            var kenoBetChoiceId = "2";

            // Act.
            var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedKenoBetChoice = "Small";

            Assert.Equal(expectedKenoBetChoice, actualKenoBetChoice);
        }

        [Fact]
        public void GetKenoBetTypeIdByValidKenoBetChoiceId()
        {
            // Arrange.
            var kenoBetChoiceId = "2";

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualKenoBetTypeId);

            // Assert.
            var expectedKenoBetTypeId = "1";

            Assert.Equal(expectedKenoBetTypeId, actualKenoBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet choice")]
        public void GetEmptyStringAsKenoBetChoiceByInvalidKenoBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            Assert.Empty(actualKenoBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: (invalid kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet type Id")]
        public void GetEmptyStringAsKenoBetTypeIdByInvalidKenoBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Empty(actualKenoBetTypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid kenoBetChoiceId | GET: given defaultValue as Keno bet choice")]
        public void TakeBackDefaultValueAsKenoBetChoiceWithInvalidKenoBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out _);

            // Assert.
            Assert.Equal(DefaultValue, actualKenoBetChoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("0")]
        [Description("GIVE: invalid kenoBetChoiceId | GET: given defaultValue as Keno bet type Id")]
        public void TakeBackDefaultValueAsKenoBetTypeIdWithInvalidKenoBetChoiceId(string kenoBetChoiceId)
        {
            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                DefaultValue,
                out string actualKenoBetTypeId);

            // Assert.
            Assert.Equal(DefaultValue, actualKenoBetTypeId);
        }
    }
}