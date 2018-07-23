namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RefControllerTestBase
    {
        private readonly string _oddsTypeId;

        public OddsTypeNameTests()
            => _oddsTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetOddsTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received().GetOddsTypeName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
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
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeOddsTypeNames();

            // Act.
            var actualResult = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedOddsTypeNames = FakeOddsTypeNames();

            // Act.
            var actualResult = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedOddsTypeNames, actualResult.Values);
        }

        private List<string> FakeOddsTypeNames()
        {
            var expectedOddsTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var oddsTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(oddsTypeNameInSpecificLanguage);
                expectedOddsTypeNames.Add(oddsTypeNameInSpecificLanguage);
            }

            return expectedOddsTypeNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeOddsTypeNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeOddsTypeNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.OddsTypeName(_oddsTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeOddsTypeNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetOddsTypeName(Arg.Is(_oddsTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}