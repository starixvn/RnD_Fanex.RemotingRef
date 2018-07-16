namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class FastMarkets225ResultTests : RefControllerBaseTests
    {
        private readonly string _resultId;

        public FastMarkets225ResultTests()
            => _resultId = Fixture.Create<string>();

        [Fact]
        public void CallGetFastMarkets225ResultOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetFastMarkets225Result(Arg.Is(_resultId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetFastMarkets225ResultOfRefDataHelper()
        {
            // Act.
            RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetFastMarkets225Result(Arg.Is(_resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeFastMarkets225Results();

            // Act.
            var actualFastMarkets225Results = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualFastMarkets225Results.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetFastMarkets225ResultOfRefDataHelper()
        {
            // Arrange.
            var expectedFastMarkets225Results = FakeFastMarkets225Results();

            // Act.
            var actualFastMarkets225Results = RefController.FastMarkets225Result(_resultId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedFastMarkets225Results, actualFastMarkets225Results);
        }

        private Dictionary<string, string> FakeFastMarkets225Results()
        {
            var expectedFastMarkets225Results = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var fastMarkets225ResultInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetFastMarkets225Result(Arg.Is(_resultId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(fastMarkets225ResultInSpecificLanguage);
                expectedFastMarkets225Results.Add(standardLanguageCode, fastMarkets225ResultInSpecificLanguage);
            }

            return expectedFastMarkets225Results;
        }
    }
}