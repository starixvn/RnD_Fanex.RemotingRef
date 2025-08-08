namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore.Models;
    using NSubstitute;
    using Xunit;

    public class BetTypeNameTests : RefSyncControllerTestBase
    {
        private readonly RequestSyncFormData _formData;
        private readonly List<ParameterSet> _parameterSets;
        private readonly List<Tuple<string, string, long?>> _parameterSetsValues;

        public BetTypeNameTests()
        {
            _parameterSets = new List<ParameterSet>();
            _parameterSetsValues = new List<Tuple<string, string, long?>>();
            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
            AppSettings.UsingVNFunction = false;
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
            _parameterSets.Add(new ParameterSet
            {
                [nameof(betTypeId)] = betTypeId,
                [nameof(resourceId)] = resourceId,
                [nameof(refNo)] = refNo
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
                            Arg.Is(standardLanguageCode));
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

        private SyncedTranslationResultSet FakeSyncedBetTypeNames()
            => FakeSyncedBetTypeNames(false, null);

        private SyncedTranslationResultSet FakeSyncedBetTypeNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedBetTypeNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeNameInMultilanguages = new TranslationSet();
                var parameterSetsValue = _parameterSetsValues[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var betTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetBetTypeName(
                            Arg.Is(parameterSetsValue.Item1),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(standardLanguageCode))
                        .Returns(betTypeNameInSpecificLanguage);

                    betTypeNameInMultilanguages.Add(standardLanguageCode, betTypeNameInSpecificLanguage);
                }

                if (ValidateTextSet(betTypeNameInMultilanguages))
                {
                    expectedSyncedBetTypeNames.Add(_parameterSets[counter], betTypeNameInMultilanguages);
                }
            }

            return expectedSyncedBetTypeNames;
        }
    }
}