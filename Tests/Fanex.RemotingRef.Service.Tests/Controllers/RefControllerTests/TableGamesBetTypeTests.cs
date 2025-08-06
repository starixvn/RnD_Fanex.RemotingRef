namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TableGamesBetTypeTests : RefControllerTestBase
    {
        private readonly string _tableGamesBetTypeId;

        public TableGamesBetTypeTests()
            => _tableGamesBetTypeId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetTableGamesBetTypeOfRefDataHelper()
        {
            // Act.
            RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetTableGamesBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetTableGamesBetType(Arg.Is(_tableGamesBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetTableGamesBetType(
                        Arg.Is(_tableGamesBetTypeId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTableGamesBetTypes();

            // Act.
            var actualResult = RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedTableGamesBetTypes = FakeTableGamesBetTypes();

            // Act.
            var actualResult = RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedTableGamesBetTypes.Values, actualResult.Values);
        }

        private Dictionary<string, object> FakeTableGamesBetTypes()
        {
            var expectedTableGamesBetTypes = new Dictionary<string, object>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var tableGamesBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetTableGamesBetType(
                        Arg.Is(_tableGamesBetTypeId),
                        Arg.Is(standardLanguageCode))
                    .Returns(tableGamesBetTypeInSpecificLanguage);

                expectedTableGamesBetTypes.Add(standardLanguageCode, tableGamesBetTypeInSpecificLanguage);
            }

            return expectedTableGamesBetTypes;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeTableGamesBetTypesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeTableGamesBetTypesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.TableGamesBetType(_tableGamesBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeTableGamesBetTypesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetTableGamesBetType(
                        Arg.Is(_tableGamesBetTypeId),
                        Arg.Is(standardLanguageCode))
                    .Returns(specialValue);
            }
        }
    }
} 