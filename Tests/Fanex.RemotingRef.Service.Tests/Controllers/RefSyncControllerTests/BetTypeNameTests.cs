namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefSyncControllerBaseTests
    {
        private readonly RequestSyncFormData _formData;
        private readonly List<Dictionary<string, object>> _parameterSets;
        private readonly List<Tuple<string, string, long?>> _parameterSetsValues;

        public BetTypeNameTests()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _parameterSetsValues = new List<Tuple<string, string, long?>>();
            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };

            PrepareFormData();
        }

        private void PrepareFormData()
        {
            AddParameterSet(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<long?>());
            AddParameterSet(
                Fixture.Create<string>(),
                null,
                Fixture.Create<long?>());
            AddParameterSet(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                null);
            AddParameterSet(
                Fixture.Create<string>(),
                null,
                null);
        }

        private void AddParameterSet(string betTypeId, string resourceId, long? refNo)
        {
            _parameterSetsValues.Add(new Tuple<string, string, long?>(betTypeId, resourceId, refNo));
            _parameterSets.Add(new Dictionary<string, object>
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId,
                ["refNo"] = refNo
            });
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

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
        public void CallRefDataHelperOncePerLanguageForEachParameterSet()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetBetTypeName(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<long?>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.BetTypeName(_formData);

            // Assert.
            foreach (var parameterSetsValue in _parameterSetsValues)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetBetTypeName(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetTypeNames();

            // Act.
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetTypeNames(true, null);

            // Act.
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedBetTypeNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.BetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedBetTypeNames(
            bool requireSpecialValue = false,
            string specialValue = null)
        {
            var expectedSyncedBetTypeNames = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeNameInMultilanguages = new Dictionary<string, string>();
                var parameterSetsValue = _parameterSetsValues[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var betTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetBetTypeName(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(betTypeNameInSpecificLanguage);
                    betTypeNameInMultilanguages.Add(standardLanguageCode, betTypeNameInSpecificLanguage);
                }

                if (betTypeNameInMultilanguages.Any(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    expectedSyncedBetTypeNames.Add(_parameterSets[counter], betTypeNameInMultilanguages);
                }
            }

            return expectedSyncedBetTypeNames;
        }
    }
}