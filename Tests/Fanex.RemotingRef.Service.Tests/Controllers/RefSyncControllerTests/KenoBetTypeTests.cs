namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class KenoBetTypeTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingKenoBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _kenoBetTypeIds;

        public KenoBetTypeTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _kenoBetTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingKenoBetTypes; counter++)
            {
                var kenoBetTypeId = Fixture.Create<string>();

                _kenoBetTypeIds.Add(kenoBetTypeId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["kenoBetTypeId"] = kenoBetTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetKenoBetTypeOfRefDataHelper()
        {
            // Act.
            RefSyncController.KenoBetType(_formData);

            // Assert.
            RefData.Received().GetKenoBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachKenoBetTypeId()
        {
            // Act.
            RefSyncController.KenoBetType(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetKenoBetType(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.KenoBetType(_formData);

            // Assert.
            foreach (var kenoBetTypeId in _kenoBetTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetKenoBetType(Arg.Is(kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetTypes();

            // Act.
            var actualResult = RefSyncController.KenoBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetTypes(true, null);

            // Act.
            var actualResult = RefSyncController.KenoBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedKenoBetTypes(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.KenoBetType(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedKenoBetTypes()
            => FakeSyncedKenoBetTypes(false, null);

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedKenoBetTypes(
            bool requireSpecialValue,
            string specialValue)
        {
            var expectedSyncedKenoBetTypes = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var kenoBetTypeNameInMultilanguages = new Dictionary<string, string>();
                var kenoBetTypeId = _kenoBetTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var kenoBetTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetKenoBetType(Arg.Is(kenoBetTypeId), Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(kenoBetTypeNameInSpecificLanguage);
                    kenoBetTypeNameInMultilanguages.Add(standardLanguageCode, kenoBetTypeNameInSpecificLanguage);
                }

                if (kenoBetTypeNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedKenoBetTypes.Add(_parameterSets[counter], kenoBetTypeNameInMultilanguages);
                }
            }

            return expectedSyncedKenoBetTypes;
        }
    }
}