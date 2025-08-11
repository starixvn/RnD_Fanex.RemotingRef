namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.NetCore;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _betTypeIds;

        public DisplayedTypeIdTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _betTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingBetTypes; counter++)
            {
                var betTypeId = Fixture.Create<string>();

                _betTypeIds.Add(betTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    [nameof(betTypeId)] = betTypeId
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
        public void AlwaysCallGetDisplayedTypeIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            RefData
                .Received()
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOnceForEachBetTypeId()
        {
            // Act.
            RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count;

            RefData
                .Received(expectedNumberOfCalls)
                .GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            foreach (var batTypeId in _betTypeIds)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData
                        .Received()
                        .GetDisplayedTypeId(Arg.Is(batTypeId));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedDisplayedTypeIds();

            // Act.
            var actualResult = RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedDisplayedTypeIds()
        {
            var expectedSyncedDisplayedBetTypeIds = new SyncedTranslationResultSet();

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeId = _betTypeIds[counter];
                var displayTypeId = Fixture.Create<string>();

                RefData
                    .GetDisplayedTypeId(Arg.Is(betTypeId))
                    .Returns(displayTypeId);

                expectedSyncedDisplayedBetTypeIds.Add(
                    _parameterSets[counter],
                    new TranslationSet
                    {
                        [DefaultLanguageCode] = displayTypeId
                    });
            }

            return expectedSyncedDisplayedBetTypeIds;
        }
    }
}