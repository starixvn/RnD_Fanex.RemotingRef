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
        public void CallGetLotteryBetTypeOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetLotteryBetTypeOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetTypes();

            // Act.
            var actualLotteryBetTypes = RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualLotteryBetTypes.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetLotteryBetTypeOfRefDataHelper()
        {
            // Arrange.
            var expectedLotteryBetTypes = FakeLotteryBetTypes();

            // Act.
            var actualLotteryBetTypes = RefController.LotteryBetType(_lotteryBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedLotteryBetTypes, actualLotteryBetTypes);
        }

        private Dictionary<string, string> FakeLotteryBetTypes()
        {
            var expectedLotteryBetTypes = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var lotteryBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(lotteryBetTypeInSpecificLanguage);
                expectedLotteryBetTypes.Add(standardLanguageCode, lotteryBetTypeInSpecificLanguage);
            }

            return expectedLotteryBetTypes;
        }
    }
}