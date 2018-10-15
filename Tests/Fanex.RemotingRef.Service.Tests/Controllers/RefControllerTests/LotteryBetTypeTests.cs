namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class LotteryBetTypeTests : RefControllerTestBase
    {
        private readonly string _lotteryBetTypeId;

        public LotteryBetTypeTests()
            => _lotteryBetTypeId = Fixture.Create<string>();

        [Fact]
        public void StillAcceptBetTypeIdAsParameter()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RefController.LotteryBetType(null, betTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Is(betTypeId), Arg.Any<string>());
        }

        [Fact]
        public void PrioritizeLotteryBetTypeIdOverBetTypeIdAsParameter()
        {
            // Arrange.
            var betTypeId = Fixture.Create<string>();

            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, betTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetLotteryBetTypeOfRefDataHelper()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetLotteryBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeLotteryBetTypes();

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedLotteryBetTypes = FakeLotteryBetTypes();

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

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

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeLotteryBetTypesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeLotteryBetTypesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.LotteryBetType(_lotteryBetTypeId, null, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeLotteryBetTypesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetLotteryBetType(Arg.Is(_lotteryBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}