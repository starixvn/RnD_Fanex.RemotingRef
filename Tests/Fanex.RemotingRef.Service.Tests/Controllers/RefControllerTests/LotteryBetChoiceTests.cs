namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RefControllerBaseTests
    {
        private readonly string _lotteryBetChoiceId;

        public LotteryBetChoiceTests()
            => _lotteryBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void CallGetLotteryBetChoiceOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Any<string>(), out _);
        }

        [Fact]
        public void SendCorrectArgumentsToGetLotteryBetChoiceOfRefDataHelper()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]), out _);
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetChoices();

            // Act.
            var actualLotteryBetChoicesAndBetTypes = RefController.LotteryBetChoice(_lotteryBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualLotteryBetChoicesAndBetTypes.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetLotteryBetChoiceOfRefDataHelper()
        {
            // Arrange.
            var expectedLotteryBetChoicesAndBetTypes = FakeLotteryBetChoices();

            // Act.
            var actualLotteryBetChoicesAndBetTypes = RefController.LotteryBetChoice(_lotteryBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLotteryBetChoicesAndBetTypes, actualLotteryBetChoicesAndBetTypes);
        }

        private Dictionary<string, Tuple<string, string>> FakeLotteryBetChoices()
        {
            var expectedLotteryBetChoicesAndBetTypes = new Dictionary<string, Tuple<string, string>>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var lotteryBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var lotteryBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]), out _)
                    .Returns(parameters =>
                    {
                        parameters[2] = lotteryBetTypeInSpecificLanguage;

                        return lotteryBetChoiceInSpecificLanguage;
                    });
                expectedLotteryBetChoicesAndBetTypes.Add(
                    standardLanguageCode,
                    new Tuple<string, string>(lotteryBetChoiceInSpecificLanguage, lotteryBetTypeInSpecificLanguage));
            }

            return expectedLotteryBetChoicesAndBetTypes;
        }
    }
}