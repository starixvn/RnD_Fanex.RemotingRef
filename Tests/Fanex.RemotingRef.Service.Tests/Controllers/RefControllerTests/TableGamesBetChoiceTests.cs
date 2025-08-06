namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class TableGamesBetChoiceTests : RefControllerTestBase
    {
        private readonly string _tableGamesBetChoiceId;

        public TableGamesBetChoiceTests()
            => _tableGamesBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetTableGamesBetChoiceOfRefDataHelper()
        {
            // Act.
            RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetTableGamesBetChoice(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetTableGamesBetChoice(Arg.Is(_tableGamesBetChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetTableGamesBetChoice(
                        Arg.Is(_tableGamesBetChoiceId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeTableGamesBetChoices();

            // Act.
            var actualResult = RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedTableGamesBetChoicesAndBetTypes = FakeTableGamesBetChoices();

            // Act.
            var actualResult = RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedTableGamesBetChoicesAndBetTypes.Values, actualResult.Values);
        }

        private Dictionary<string, object> FakeTableGamesBetChoices()
        {
            var expectedTableGamesBetChoicesAndBetTypes = new Dictionary<string, object>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var tableGamesBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var tableGamesBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetTableGamesBetChoice(
                        Arg.Is(_tableGamesBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns((tableGamesBetChoiceInSpecificLanguage, tableGamesBetTypeInSpecificLanguage));

                expectedTableGamesBetChoicesAndBetTypes.Add(standardLanguageCode, (tableGamesBetChoiceInSpecificLanguage, tableGamesBetTypeInSpecificLanguage));
            }

            return expectedTableGamesBetChoicesAndBetTypes;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeTableGamesBetChoicesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeTableGamesBetChoicesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.TableGamesBetChoice(_tableGamesBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeTableGamesBetChoicesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetTableGamesBetChoice(
                        Arg.Is(_tableGamesBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(("", specialValue));
            }
        }
    }
} 