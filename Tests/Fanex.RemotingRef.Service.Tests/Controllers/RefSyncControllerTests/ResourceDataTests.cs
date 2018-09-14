namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using Fanex.RemotingRef.Service.Models;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RefSyncControllerTestBase
    {
        private const int NumberOfTestingResources = 5;

        private RequestSyncFormData _formData;
        private List<ParameterSet> _parameterSets;
        private List<Tuple<string, string>> _refIdAndResourceIdPairs;

        public ResourceDataTests()
            => PrepareFormData();

        private void PrepareFormData()
        {
            _parameterSets = new List<ParameterSet>();
            _refIdAndResourceIdPairs = new List<Tuple<string, string>>();

            for (var counter = 0; counter < NumberOfTestingResources; counter++)
            {
                var refId = Fixture.Create<string>();
                var resourceId = Fixture.Create<string>();

                _refIdAndResourceIdPairs.Add(new Tuple<string, string>(refId, resourceId));
                _parameterSets.Add(new ParameterSet
                {
                    ["refId"] = refId,
                    ["resourceId"] = resourceId
                });
            }

            _formData = new RequestSyncFormData
            {
                ParameterSets = _parameterSets,
                Languages = StandardLanguageCodes
            };
        }

        [Fact]
        public void AlwaysGetResourceDataByRefIdAndResourceIdOfRefDataHelper()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            RefData.Received().GetResourceDataByRefIdAndResourceId(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguageForEachPairOfRefIdAndResourceId()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            var expectedNumberOfCalls = _parameterSets.Count * StandardLanguageCodes.Length;

            RefData.Received(expectedNumberOfCalls).GetResourceDataByRefIdAndResourceId(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToRefDataHelper()
        {
            // Act.
            RefSyncController.ResourceData(_formData);

            // Assert.
            foreach (var pair in _refIdAndResourceIdPairs)
            {
                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    RefData.Received().GetResourceDataByRefIdAndResourceId(
                        Arg.Is(pair.Item1),
                        Arg.Is(pair.Item2),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
                }
            }
        }

        [Fact]
        public void ReturnDataGotFromRefDataHelper()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData();

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void NullIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData(true, null);

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void EmptyStringIsNotIncludedInResult()
        {
            // Arrange.
            var expectedResult = FakeSyncedResourceData(true, string.Empty);

            // Act.
            var actualResult = RefSyncController.ResourceData(_formData);

            // Assert.
            Assert.Equal(expectedResult, actualResult);
        }

        private SyncedTranslationResultSet FakeSyncedResourceData()
            => FakeSyncedResourceData(false, null);

        private SyncedTranslationResultSet FakeSyncedResourceData(bool requireSpecialValue, string specialValue)
        {
            var expectedSyncedResourceData = new SyncedTranslationResultSet();
            var specialIndex = -1;

            if (requireSpecialValue)
            {
                specialIndex = new Random().Next(0, _parameterSets.Count - 1);
            }

            for (var counter = 1; counter < _parameterSets.Count; counter++)
            {
                var resourceInMultilanguages = new TranslationSet();
                var refIdAndResourceIdPair = _refIdAndResourceIdPairs[counter];
                var refId = refIdAndResourceIdPair.Item1;
                var resourceId = refIdAndResourceIdPair.Item2;

                foreach (var standardLanguageCode in StandardLanguageCodes)
                {
                    var resourceInSpecificLanguage = counter == specialIndex ? specialValue : Fixture.Create<string>();

                    RefData
                        .GetResourceDataByRefIdAndResourceId(
                            Arg.Is(refId),
                            Arg.Is(resourceId),
                            Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                        .Returns(resourceInSpecificLanguage);
                    resourceInMultilanguages.Add(standardLanguageCode, resourceInSpecificLanguage);
                }

                if (ValidateTextSet(resourceInMultilanguages))
                {
                    expectedSyncedResourceData.Add(_parameterSets[counter], resourceInMultilanguages);
                }
            }

            return expectedSyncedResourceData;
        }
    }
}