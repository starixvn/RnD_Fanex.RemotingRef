namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using NSubstitute;
    using Xunit;

    public class OddsTypeNameTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingOddsTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _oddsTypeIds;

        public OddsTypeNameTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _oddsTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingOddsTypes; counter++)
            {
                var oddsTypeId = Fixture.Create<string>();

                _oddsTypeIds.Add(oddsTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(oddsTypeId)] = oddsTypeId
                });
            }

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
        }

        [Fact]
        public void AlwaysCallGetOddsTypeNameOfRefDataHelper()
        {
            // Act.
            RefSyncController.OddsTypeName(_formData);

            // Assert.
            RefData
                .Received()
                .GetOddsTypeName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachOddsTypeId()
        {
            // Act.
            RefSyncController.OddsTypeName(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData
                .Received(expectedNumberOfCalls)
                .GetOddsTypeName(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.OddsTypeName(_formData);

            // Assert.
            foreach (var oddsTypeId in _oddsTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetOddsTypeName(Arg.Is(oddsTypeId), Arg.Is(standardLanguageCode));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedOddsTypeNames();

            // Act.
            var actualResult = RefSyncController.OddsTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedOddsTypeNames(true, null);

            // Act.
            var actualResult = RefSyncController.OddsTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedOddsTypeNames(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.OddsTypeName(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedOddsTypeNames()
            => FakeSyncedOddsTypeNames(false, null);

        private SyncedTranslationResultSet FakeSyncedOddsTypeNames(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedOddsTypeNames = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var oddsNameInMultilanguages = new TranslationSet();
                var oddsTypeId = _oddsTypeIds[counter];

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var oddsNameInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetOddsTypeName(Arg.Is(oddsTypeId), Arg.Is(standardLanguageCode))
                        .Returns(oddsNameInSpecificLanguage);

                    oddsNameInMultilanguages.Add(standardLanguageCode, oddsNameInSpecificLanguage);
                }

                if (ValidateTextSet(oddsNameInMultilanguages))
                {
                    expectedSyncedOddsTypeNames.Add(_parameterSets[counter], oddsNameInMultilanguages);
                }
            }

            return expectedSyncedOddsTypeNames;
        }
    }
}