namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RefControllerBaseTests
    {
        private readonly string _oddsTypeId;

        public OddsTypeNameTests()
            => _oddsTypeId = Fixture.Create<string>();

        [Fact]
        public void CallGetOddsTypeNameOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetOddsTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeOddsTypeNames();

            // Act.
            var actualOddsTypeNames = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualOddsTypeNames.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetOddsTypeNameOfRefDataHelper()
        {
            // Arrange.
            var expectedOddsTypeNames = FakeOddsTypeNames();

            // Act.
            var actualOddsTypeNames = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedOddsTypeNames, actualOddsTypeNames);
        }

        private Dictionary<string, string> FakeOddsTypeNames()
        {
            var expectedOddsTypeNames = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var oddsTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(oddsTypeNameInSpecificLanguage);
                expectedOddsTypeNames.Add(standardLanguageCode, oddsTypeNameInSpecificLanguage);
            }

            return expectedOddsTypeNames;
        }
    }
}