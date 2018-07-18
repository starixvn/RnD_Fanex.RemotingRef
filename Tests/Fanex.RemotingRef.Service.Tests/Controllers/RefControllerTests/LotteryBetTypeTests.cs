namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RefControllerBaseTests
    {
        private readonly string _lotteryBetTypeId;

        public LotteryBetTypeTests()
            => _lotteryBetTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetLotteryBetTypeOfRefDataHelper()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received().GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetTypes();

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLotteryBetTypes = FakeLotteryBetTypes();

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLotteryBetTypes, actualResult.Values);
        }

        private List<string> FakeLotteryBetTypes()
        {
            var expectedLotteryBetTypes = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var lotteryBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(lotteryBetTypeInSpecificLanguage);
                expectedLotteryBetTypes.Add(lotteryBetTypeInSpecificLanguage);
            }

            return expectedLotteryBetTypes;
        }
    }
}