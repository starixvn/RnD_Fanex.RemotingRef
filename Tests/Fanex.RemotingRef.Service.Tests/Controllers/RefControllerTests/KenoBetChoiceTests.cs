namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetChoiceTests : RefControllerBaseTests
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
            RefData.Received().GetKenoBetChoice(Arg.Any<string>(), Arg.Any<string>(), out _);
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetKenoBetChoice(Arg.Is(_kenoBetChoiceId), Arg.Any<string>(), out _);
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
                    .GetKenoBetChoice(Arg.Is(_kenoBetChoiceId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]), out _);
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

            // Assert.
            Assert.Equal(expectedKenoBetChoicesAndBetTypes, actualResult.Values);
        }

        private List<Tuple<string, string>> FakeKenoBetChoices()
        {
            var expectedKenoBetChoicesAndBetTypes = new List<Tuple<string, string>>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var kenoBetChoiceInSpecificLanguage = Fixture.Create<string>();
                var kenoBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetKenoBetChoice(Arg.Is(_kenoBetChoiceId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]), out _)
                    .Returns(parameters =>
                    {
                        parameters[2] = kenoBetTypeInSpecificLanguage;

                        return kenoBetChoiceInSpecificLanguage;
                    });
                expectedKenoBetChoicesAndBetTypes.Add(
                    new Tuple<string, string>(kenoBetChoiceInSpecificLanguage, kenoBetTypeInSpecificLanguage));
            }

            return expectedKenoBetChoicesAndBetTypes;
        }
    }
}