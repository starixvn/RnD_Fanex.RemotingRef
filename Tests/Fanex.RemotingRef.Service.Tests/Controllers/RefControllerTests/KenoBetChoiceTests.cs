namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetChoiceTests : RefControllerTestBase
    {
        private readonly string _kenoBetChoiceId;

        public KenoBetChoiceTests()
            => _kenoBetChoiceId = Fixture.Create<string>();

        [Fact]
        public void AlwaysCallGetKenoBetChoiceOfRefDataHelper()
        {
            // Act.
            RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetKenoBetChoice(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetKenoBetChoice(Arg.Is(_kenoBetChoiceId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetKenoBetChoice(
                        Arg.Is(_kenoBetChoiceId),
                        Arg.Is(standardLanguageCode));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeKenoBetChoices();

            // Act.
            var actualResult = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedKenoBetChoicesAndBetTypes = FakeKenoBetChoices();

            // Act.
            var actualResult = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert
            Assert.Equal(expectedKenoBetChoicesAndBetTypes.Values, actualResult.Values);
        }

        private Dictionary<string, object> FakeKenoBetChoices()
        {
            var expectedKenoBetChoicesAndBetTypes = new Dictionary<string, object>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var kenoBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var kenoBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetKenoBetChoice(
                        Arg.Is(_kenoBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns((kenoBetChoiceInSpecificLanguage, kenoBetTypeInSpecificLanguage));

                expectedKenoBetChoicesAndBetTypes.Add(standardLanguageCode, (kenoBetChoiceInSpecificLanguage, kenoBetTypeInSpecificLanguage));
            }

            return expectedKenoBetChoicesAndBetTypes;
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeKenoBetChoicesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        [Fact]
        public void ReturnEmptyIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeKenoBetChoicesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.True(actualResult.Count == 0);
        }

        private void FakeKenoBetChoicesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetKenoBetChoice(
                        Arg.Is(_kenoBetChoiceId),
                        Arg.Is(standardLanguageCode))
                    .Returns(("", specialValue));
            }
        }
    }
}