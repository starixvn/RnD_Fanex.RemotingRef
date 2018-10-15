namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetChoiceTests : RefControllerTestBase
    {
        private readonly string _lotteryBetChoiceId;

        public LotteryBetChoiceTests()
            => _lotteryBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void StillAcceptBetChoiceIdAsParameter()
        {
            // Arrange.
            var betChoiceId = Fixture.Create<string>();

            // Act.
            RefController.LotteryBetChoice(null, betChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Is(betChoiceId), Arg.Any<string>(), out _);
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
                .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Any<string>(), out _);
        }

        [Fact]
        public void AlwaysCallGetLotteryBetChoiceOfRefDataHelper()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetChoice(Arg.Any<string>(), Arg.Any<string>(), out _);
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetLotteryBetChoice(Arg.Is(_lotteryBetChoiceId), Arg.Any<string>(), out _);
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                        out _);
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetChoices();

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLotteryBetChoicesAndBetTypes = FakeLotteryBetChoices();

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLotteryBetChoicesAndBetTypes, actualResult.Values);
        }

        private List<Tuple<string, string>> FakeLotteryBetChoices()
        {
            var expectedLotteryBetChoicesAndBetTypes = new List<Tuple<string, string>>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var lotteryBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var lotteryBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                        out _)
                    .Returns(parameters =>
                    {
                        parameters[2] = lotteryBetTypeInSpecificLanguage;

                        return lotteryBetChoiceInSpecificLanguage;
                    });

                expectedLotteryBetChoicesAndBetTypes.Add(
                    new Tuple<string, string>(lotteryBetChoiceInSpecificLanguage, lotteryBetTypeInSpecificLanguage));
            }

            return expectedLotteryBetChoicesAndBetTypes;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeLotteryBetChoicesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeLotteryBetChoicesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.LotteryBetChoice(_lotteryBetChoiceId, null, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeLotteryBetChoicesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetLotteryBetChoice(
                        Arg.Is(_lotteryBetChoiceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]),
                        out _)
                    .Returns(parameters =>
                    {
                        parameters[2] = specialValue;

                        return specialValue;
                    });
            }
        }
    }
}