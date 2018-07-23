namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefControllerTestBase
    {
        private readonly string _betTypeId;
        private readonly string _resourceId;
        private readonly long? _refNo;

        public BetTypeNameTests()
        {
            _betTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.BetTypeName(_betTypeId, _resourceId, _refNo, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.BetTypeName(_betTypeId, _resourceId, _refNo, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetBetTypeName(
                    Arg.Is(_betTypeId),
                    Arg.Is(_resourceId),
                    Arg.Is(_refNo),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeBetTypeNames();

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedBetTypeNames = FakeBetTypeNames();

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetTypeNames, actualResult.Values);
        }

        private List<string> FakeBetTypeNames()
        {
            var expectedBetTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var betTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(betTypeNameInSpecificLanguage);
                expectedBetTypeNames.Add(betTypeNameInSpecificLanguage);
            }

            return expectedBetTypeNames;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeBetTypeNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeBetTypeNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.BetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeBetTypeNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}