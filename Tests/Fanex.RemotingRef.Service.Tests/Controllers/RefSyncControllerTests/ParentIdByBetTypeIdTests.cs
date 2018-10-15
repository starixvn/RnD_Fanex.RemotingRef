namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class ParentIdByBetTypeIdTests : RefSyncControllerTestBase
    {
        private const int NumberOfChildBetTypes = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<string> _childBetTypeIds;

        public ParentIdByBetTypeIdTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _childBetTypeIds = new List<string>();

            for (var counter = 0; counter < NumberOfChildBetTypes; counter++)
            {
                var childBetTypeId = Fixture.Create<string>();

                _childBetTypeIds.Add(childBetTypeId);
                _parameterSets.Add(new ParameterSet
                {
                    ["betTypeId"] = childBetTypeId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysCallGetParentIdByBetTypeIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            RefData
                .Received()
                .GetParentIdByBetTypeId(Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerBetTypeId()
        {
            // Act.
            RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count;

            RefData
                .Received(expectedNumberOfCalls)
                .GetParentIdByBetTypeId(Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            foreach (var betTypeId in _childBetTypeIds)
            {
                RefData
                    .Received()
                    .GetParentIdByBetTypeId(Arg.Is(betTypeId));
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedParentBetTypeIds();

            // Act.
            var actualResult = RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedParentBetTypeIds(true, null);

            // Act.
            var actualResult = RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedParentBetTypeIds(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.ParentIdByBetTypeId(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedParentBetTypeIds()
            => FakeSyncedParentBetTypeIds(false, null);

        private SyncedTranslationResultSet FakeSyncedParentBetTypeIds(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedParentBetTypeIds = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var betTypeId = _childBetTypeIds[counter];
                var parentBetTypeId = counter == specialIndex ? specialValue : Fixture.Create<string>();

                RefData.GetParentIdByBetTypeId(Arg.Is(betTypeId)).Returns(parentBetTypeId);

                if (!string.IsNullOrEmpty(parentBetTypeId))
                {
                    expectedSyncedParentBetTypeIds.Add(
                        _parameterSets[counter],
                        new TranslationSet
                        {
                            [DefaultLanguageCode] = parentBetTypeId
                        });
                }
            }

            return expectedSyncedParentBetTypeIds;
        }
    }
}