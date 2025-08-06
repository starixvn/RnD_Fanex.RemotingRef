namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class KenoBetChoiceTests : ServiceIntegrationTestBase
    {
        [Fact]
        [Description("GIVE: existing kenoBetChoiceId | GET: expected Keno bet choice")]
        public void GetKenoBetChoiceByExistingKenoBetChoiceId()
        {
            // Arrange.
            const string existingKenoBetChoiceId = "2";

            // Act.
            var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
                existingKenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            const string expectedKenoBetChoice = "Small";

            Assert.Equal(expectedKenoBetChoice, actualKenoBetChoice);
        }

        [Fact]
        [Description("GIVE: existing kenoBetChoiceId | GET: expected Keno bet type ID")]
        public void GetKenoBetTypeIdByExistingKenoBetChoiceId()
        {
            // Arrange.
            const string existingKenoBetChoiceId = "2";

            // Act.
            RemotingRefService.GetKenoBetChoice(
                existingKenoBetChoiceId,
                TestingLanguage,
                out string actualKenoBetTypeId);

            // Assert.
            const string expectedKenoBetTypeId = "1";

            Assert.Equal(expectedKenoBetTypeId, actualKenoBetTypeId);
        }

        //[Fact]
        //[Description("GIVE: (unexisting kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet choice")]
        //public void GetEmptyStringAsKenoBetChoiceByUnexistingKenoBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingKenoBetChoiceId = "-1";

        //    // Act.
        //    var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
        //        unexistingKenoBetChoiceId,
        //        TestingLanguage,
        //        out _);

        //    // Assert.
        //    Assert.Empty(actualKenoBetChoice);
        //}

        //[Fact]
        //[Description("GIVE: (unexisting kenoBetChoiceId) && (no defaultValue provided) | GET: empty string as Keno bet type ID")]
        //public void GetEmptyStringAsKenoBetTypeIdByUnexistingKenoBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingKenoBetChoiceId = "-1";

        //    // Act.
        //    RemotingRefService.GetKenoBetChoice(
        //        unexistingKenoBetChoiceId,
        //        TestingLanguage,
        //        out string actualKenoBetTypeId);

        //    // Assert.
        //    Assert.Empty(actualKenoBetTypeId);
        //}

        //[Fact]
        //[Description("GIVE: unexisting kenoBetChoiceId | GET: given defaultValue as Keno bet choice")]
        //public void TakeBackDefaultValueAsKenoBetChoiceWithUnexistingKenoBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingKenoBetChoiceId = "-2";

        //    // Act.
        //    var actualKenoBetChoice = RemotingRefService.GetKenoBetChoice(
        //        unexistingKenoBetChoiceId,
        //        TestingLanguage,
        //        DefaultValue,
        //        out _);

        //    // Assert.
        //    Assert.Equal(DefaultValue, actualKenoBetChoice);
        //}

        //[Fact]
        //[Description("GIVE: unexisting kenoBetChoiceId | GET: given defaultValue as Keno bet type ID")]
        //public void TakeBackDefaultValueAsKenoBetTypeIdWithUnexistingKenoBetChoiceId()
        //{
        //    // Arrange.
        //    const string unexistingKenoBetChoiceId = "-2";

        //    // Act.
        //    RemotingRefService.GetKenoBetChoice(
        //        unexistingKenoBetChoiceId,
        //        TestingLanguage,
        //        DefaultValue,
        //        out string actualKenoBetTypeId);

        //    // Assert.
        //    Assert.Equal(DefaultValue, actualKenoBetTypeId);
        //}
    }
}