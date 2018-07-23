namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RefControllerTestBase
    {
        private readonly string _refId;
        private readonly string _resourceId;

        public ResourceDataTests()
        {
            _refId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
        }

        [Fact]
        public void AlwaysCallGetResourceDataOfRefDataHelper()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received()
                .GetResourceDataByRefIdAndResourceId(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetResourceDataByRefIdAndResourceId(Arg.Is(_refId), Arg.Is(_resourceId), Arg.Any<string>());
        }

        [Fact]
        public void CallRefDataHelperWithCorrectArguments()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .Received()
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]));
            }
        }

        [Fact]
        public void ResultTakesStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeResources();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedResourceData = FakeResources();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedResourceData, actualResult.Values);
        }

        private List<string> FakeResources()
        {
            var expectedResourceData = new List<string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var resourceInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(resourceInSpecificLanguage);
                expectedResourceData.Add(resourceInSpecificLanguage);
            }

            return expectedResourceData;
        }

        [Fact]
        public void ReturnNullIfResultContainsAllNullValues()
        {
            // Arrange.
            FakeResourcesWithSpecialValue(null);

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        [Fact]
        public void ReturnNullIfResultContainsAllEmptyStrings()
        {
            // Arrange.
            FakeResourcesWithSpecialValue(string.Empty);

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Null(actualResult);
        }

        private void FakeResourcesWithSpecialValue(string specialValue)
        {
            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                RefData
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(specialValue);
            }
        }
    }
}