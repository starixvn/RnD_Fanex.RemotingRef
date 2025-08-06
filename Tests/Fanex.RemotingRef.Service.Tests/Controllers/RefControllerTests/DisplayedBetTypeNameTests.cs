namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class DisplayedBetTypeNameTests : RefControllerTestBase
    {
        private readonly string _betTypeId;
        private readonly string _displayedBetTypeId;
        private readonly string _resourceId;
        private readonly long? _refNo;

        public DisplayedBetTypeNameTests()
        {
            _betTypeId = Fixture.Create<string>();
            _displayedBetTypeId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
            _refNo = Fixture.Create<long>();

            RefData
                .GetDisplayedTypeId(Arg.Is(_betTypeId))
                .Returns(_displayedBetTypeId);
        }

        [Fact]
        public void AlwaysCallGetDisplayedTypeIdOfRefDataHelper()
        {
            // Act.
            RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetDisplayedTypeIdOnlyOnce()
        {
            // Act.
            RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received(1)
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

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
        public void CallGettingBetTypeNameOncePerLanguage()
        {
            // Act.
            RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetBetTypeName(
                    Arg.Is(_displayedBetTypeId),
                    Arg.Is(_resourceId),
                    Arg.Is(_refNo),
                    Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.DisplayedBetTypeName(
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
                        Arg.Is(_displayedBetTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeDisplayedBetTypeNames();

            // Act.
            var actualResult = RefController.DisplayedBetTypeName(
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
            var expectedBetTypeNames = FakeDisplayedBetTypeNames();

            // Act.
            var actualResult = RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedBetTypeNames, actualResult.Values);
        }

        [Fact]
        public void NeverReturnOriginalBetTypeNamesAsValues()
        {
            // Arrange.
            var originalBetTypeNames = FakeOriginalBetTypeNames();

            FakeDisplayedBetTypeNames();

            // Act.
            var actualResult = RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.NotEqual(originalBetTypeNames, actualResult.Values);
        }

        private List<string> FakeOriginalBetTypeNames()
        {
            var originalBetTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var originalBetTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetTypeName(
                        Arg.Is(_betTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo), 
                        Arg.Is(standardLanguageCode))
                    .Returns(originalBetTypeNameInSpecificLanguage);

                originalBetTypeNames.Add(originalBetTypeNameInSpecificLanguage);
            }

            return originalBetTypeNames;
        }

        private List<string> FakeDisplayedBetTypeNames()
        {
            var expectedDisplayedBetTypeNames = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var displayedBetTypeNameInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetBetTypeName(
                        Arg.Is(_displayedBetTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(displayedBetTypeNameInSpecificLanguage);

                expectedDisplayedBetTypeNames.Add(displayedBetTypeNameInSpecificLanguage);
            }

            return expectedDisplayedBetTypeNames;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeDisplayedBetTypeNamesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeDisplayedBetTypeNamesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.DisplayedBetTypeName(
                _betTypeId,
                _resourceId,
                _refNo,
                StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeDisplayedBetTypeNamesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetBetTypeName(
                        Arg.Is(_displayedBetTypeId),
                        Arg.Is(_resourceId),
                        Arg.Is(_refNo),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
}