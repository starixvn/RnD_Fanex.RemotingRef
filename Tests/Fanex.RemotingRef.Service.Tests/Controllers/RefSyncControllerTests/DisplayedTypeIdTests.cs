namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class DisplayedTypeIdTests : RefSyncControllerBaseTests
    {
        private const int NumberOfTestingBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<Dictionary<string, object>> _parameterSets;
        private List<string> _betTypeIds;

        public DisplayedTypeIdTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<Dictionary<string, object>>();
            _betTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfTestingBetTypes; counter++)
            {
                var betTypeId = Fixture.Create<string>();

                _betTypeIds.Add(betTypeId);
                _parameterSets.Add(new Dictionary<string, object>
                {
                    ["betTypeId"] = betTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets
            };
        }

        [Fact]
        public void AlwaysCallGetDisplayedTypeIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            RefData.Received().GetDisplayedTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOnceForEachBetTypeId()
        {
            // Act.
            RefSyncController.DisplayedTypeId(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count;

            RefData.Received(expectedNumberOfCalls).GetDisplayedTypeId(Arg.Any<string>());
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
                    RefData.Received().GetDisplayedTypeId(Arg.Is(batTypeId));
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

        private Dictionary<Dictionary<string, object>, Dictionary<string, string>> FakeSyncedDisplayedTypeIds()
        {
            var expectedSyncedDisplayBetTypeIds = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>();

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeId = _betTypeIds[counter];
                var displayTypeId = Fixture.Create<string>();

                RefData.GetDisplayedTypeId(Arg.Is(betTypeId)).Returns(displayTypeId);
                expectedSyncedDisplayBetTypeIds.Add(
                    _parameterSets[counter],
                    new Dictionary<string, string>
                    {
                        [DefaultLanguageCode] = displayTypeId
                    });
            }

            return expectedSyncedDisplayBetTypeIds;
        }
    }
}