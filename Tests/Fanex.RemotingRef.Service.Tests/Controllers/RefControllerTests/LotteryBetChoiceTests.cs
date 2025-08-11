namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Controllers;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RefControllerTestBase
    {
        private readonly string _lotteryBetChoiceId;

        public LotteryBetChoiceTests()
        {
            _lotteryBetChoiceId = Fixture.Create<string>();
            RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
        }

        [Fact]
        public void StillAcceptBetChoiceIdAsParameter()
        {
            // Arrange.
            var betChoiceId = Fixture.Create<string>();

            // Act.
            RefController.LotteryBetChoice(betChoiceId: betChoiceId, languages: StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Is(betChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void PrioritizeLotteryBetChoiceIdOverChoiceIdAsParameter()
        {
            // Arrange.
            var betChoiceId = Fixture.Create<string>();

            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, betChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetLotteryBetChoiceOfRefDataHelper()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetChoices();

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLotteryBetChoicesAndBetTypes = FakeLotteryBetChoices();

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLotteryBetChoicesAndBetTypes.Values, actualResult.Values);
        }

        private Dictionary<string, object> FakeLotteryBetChoices()
        {
            var expectedLotteryBetChoicesAndBetTypes = new Dictionary<string, object>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var lotteryBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var lotteryBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns((lotteryBetChoiceInSpecificLanguage, lotteryBetTypeInSpecificLanguage));

                expectedLotteryBetChoicesAndBetTypes.Add(standardLanguageCode, (lotteryBetChoiceInSpecificLanguage, lotteryBetTypeInSpecificLanguage));
            }

            return expectedLotteryBetChoicesAndBetTypes;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeLotteryBetChoicesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeLotteryBetChoicesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, languages: StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeLotteryBetChoicesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns((specialValue, specialValue));
            }
        }
    }
}