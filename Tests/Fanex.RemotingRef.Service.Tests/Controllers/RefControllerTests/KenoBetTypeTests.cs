namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RefControllerBaseTests
    {
        private readonly string _kenoBetTypeId;

        public KenoBetTypeTests()
            => _kenoBetTypeId = Fixture.Create<string>();

        [Fact]
        public void CallGetKenoBetTypeOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            RefData.Received(StandardLanguageCodes.Length).GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetKenoBetTypeOfRefDataHelper()
        {
            // Act.
            RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData.Received().GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeKenoBetTypes();

            // Act.
            var actualKenoBetTypes = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualKenoBetTypes.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetKenoBetTypeOfRefDataHelper()
        {
            // Arrange.
            var expectedKenoBetTypes = FakeKenoBetTypes();

            // Act.
            var actualKenoBetTypes = RefController.KenoBetType(_kenoBetTypeId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedKenoBetTypes, actualKenoBetTypes);
        }

        private Dictionary<string, string> FakeKenoBetTypes()
        {
            var expectedKenoBetTypes = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var kenoBetTypeInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetKenoBetType(Arg.Is(_kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(kenoBetTypeInSpecificLanguage);
                expectedKenoBetTypes.Add(standardLanguageCode, kenoBetTypeInSpecificLanguage);
            }

            return expectedKenoBetTypes;
        }
    }
}