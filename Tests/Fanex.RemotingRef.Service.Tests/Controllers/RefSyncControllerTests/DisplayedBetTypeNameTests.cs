namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class DisplayedBetTypeNameTests : RefSyncControllerTestBase
    {
        private readonly RequestSyncFormData _formData;
        private readonly List<ParameterSet> _parameterSets;
        private readonly List<Tuple<string, string, long?>> _parameterSetsValues;
        private readonly List<string> _displayedBetTypeIds;

        public DisplayedBetTypeNameTests()
        {
            _parameterSets = new List<ParameterSet>();
            _parameterSetsValues = new List<Tuple<string, string, long?>>();
            _displayedBetTypeIds = new List<string>();
            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
            var fakeAppSettings = new AppSettings
            {
                UsingVNFunction = false
            };
            AppSettings.Value.Returns(fakeAppSettings);
            RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);
            PrepareFormData();
        }

        private void PrepareFormData()
        {
            AddParameterSetAndRegisterRefData(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                Fixture.Create<long?>());
            AddParameterSetAndRegisterRefData(
                Fixture.Create<string>(),
                null,
                Fixture.Create<long?>());
            AddParameterSetAndRegisterRefData(
                Fixture.Create<string>(),
                Fixture.Create<string>(),
                null);
            AddParameterSetAndRegisterRefData(
                Fixture.Create<string>(),
                null,
                null);
        }

        private void AddParameterSetAndRegisterRefData(string betTypeId, string resourceId, long? refNo)
        {
            var displayedBetTypeId = Fixture.Create<string>();

            _parameterSetsValues.Add(new Tuple<string, string, long?>(betTypeId, resourceId, refNo));
            _parameterSets.Add(new ParameterSet
            {
                [nameof(betTypeId)] = betTypeId,
                [nameof(resourceId)] = resourceId,
                [nameof(refNo)] = refNo
            });
            _displayedBetTypeIds.Add(displayedBetTypeId);
            RefData.GetDisplayedTypeId(Arg.Is(betTypeId)).Returns(displayedBetTypeId);
        }

        [Fact]
        public void AlwaysCallGetDisplayedTypeIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            RefData
                .Received()
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallGetDisplayedTypeIdOncePerParameterSet()
        {
            // Act.
            RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count;

            RefData
                .Received(expectedNumberOfCalls)
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void AlwaysCallGetBetTypeNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.DisplayedBetTypeName(_formData);

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
        public void CallGettingBetTypeNameOncePerLanguageForEachParameterSet()
        {
            // Act.
            RefSyncController.DisplayedBetTypeName(_formData);

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
            RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            for (var counter = 0; counter < _parameterSetsValues.Count; counter++)
            {
                var parameterSetsValue = _parameterSetsValues[counter];
                var displayedBetTypeId = _displayedBetTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetBetTypeName(
                            Arg.Is(displayedBetTypeId),
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
            var expectedResult = FakeSyncedDisplayedBetTypeNames();

            // Act.
            var actualResult = RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedDisplayedBetTypeNames(true, null);

            // Act.
            var actualResult = RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedDisplayedBetTypeNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.DisplayedBetTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedDisplayedBetTypeNames()
            => FakeSyncedDisplayedBetTypeNames(false, null);

        private SyncedTranslationResultSet FakeSyncedDisplayedBetTypeNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedDisplayedBetTypeNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var displayedBetTypeNameInMultilanguages = new TranslationSet();
                var parameterSetsValue = _parameterSetsValues[counter];
                var displayedBetTypeId = _displayedBetTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var displayedBetTypeNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetBetTypeName(
                            Arg.Is(displayedBetTypeId),
                            Arg.Is(parameterSetsValue.Item2),
                            Arg.Is(parameterSetsValue.Item3),
                            Arg.Is(standardLanguageCode))
                        .Returns(displayedBetTypeNameInSpecificLanguage);

                    displayedBetTypeNameInMultilanguages.Add(standardLanguageCode, displayedBetTypeNameInSpecificLanguage);
                }

                if (ValidateTextSet(displayedBetTypeNameInMultilanguages))
                {
                    expectedSyncedDisplayedBetTypeNames.Add(_parameterSets[counter], displayedBetTypeNameInMultilanguages);
                }
            }

            return expectedSyncedDisplayedBetTypeNames;
        }
    }
}