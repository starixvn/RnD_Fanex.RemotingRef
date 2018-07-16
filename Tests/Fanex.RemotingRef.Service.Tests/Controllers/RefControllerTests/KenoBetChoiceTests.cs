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
        public void CallGetKenoBetChoiceOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetKenoBetChoice(Arg.Is(_kenoBetChoiceId), Arg.Any<string>(), out _);
        }

        [Fact]
        public void SendCorrectArgumentsToGetKenoBetChoiceOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeKenoBetChoices();

            // Act.
            var actualKenoBetChoicesAndBetTypes = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualKenoBetChoicesAndBetTypes.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetKenoBetChoiceOfRefDataHelper()
        {
            // Arrange.
            var expectedKenoBetChoicesAndBetTypes = FakeKenoBetChoices();

            // Act.
            var actualKenoBetChoicesAndBetTypes = RefController.KenoBetChoice(_kenoBetChoiceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedKenoBetChoicesAndBetTypes, actualKenoBetChoicesAndBetTypes);
        }

        private Dictionary<string, Tuple<string, string>> FakeKenoBetChoices()
        {
            var expectedKenoBetChoicesAndBetTypes = new Dictionary<string, Tuple<string, string>>();

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
                    standardLanguageCode,
                    new Tuple<string, string>(kenoBetChoiceInSpecificLanguage, kenoBetTypeInSpecificLanguage));
            }

            return expectedKenoBetChoicesAndBetTypes;
        }
    }
}