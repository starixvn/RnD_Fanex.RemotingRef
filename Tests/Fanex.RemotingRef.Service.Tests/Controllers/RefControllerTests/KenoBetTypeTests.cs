namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RefControllerTestBase
    {
        private readonly string _kenoBetTypeId;

        public KenoBetTypeTests()
            => _kenoBetTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetKenoBetTypeOfRefDataHelper()
        {
            // Act.
            RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetKenoBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeKenoBetTypes();

            // Act.
            var actualResult = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedKenoBetTypes = FakeKenoBetTypes();

            // Act.
            var actualResult = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedKenoBetTypes, actualResult.Values);
        }

        private List<string> FakeKenoBetTypes()
        {
            var expectedKenoBetTypes = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var kenoBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(kenoBetTypeInSpecificLanguage);

                expectedKenoBetTypes.Add(kenoBetTypeInSpecificLanguage);
            }

            return expectedKenoBetTypes;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeKenoBetTypesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeKenoBetTypesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeKenoBetTypesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}