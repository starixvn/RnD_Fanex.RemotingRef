namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using System.Collections.Generic;
    using AutoFixture;
    using NSubstitute;
    using Xunit;

    public class ResourceDataTests : RefControllerBaseTests
    {
        private readonly string _refId;
        private readonly string _resourceId;

        public ResourceDataTests()
        {
            _refId = Fixture.Create<string>();
            _resourceId = Fixture.Create<string>();
        }

        [Fact]
        public void CallGetResourceDataByRefIdAndResourceIdOfRefDataHelperOncePerLanguage()
        {
            // Act.
            RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            RefData
                .Received(StandardLanguageCodes.Length)
                .GetResourceDataByRefIdAndResourceId(Arg.Is(_refId), Arg.Is(_resourceId), Arg.Any<string>());
        }

        [Fact]
        public void SendCorrectArgumentsToGetResourceDataByRefIdAndResourceIdOfRefDataHelper()
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
        public void ReturnResultWithStandardLanguageCodesAsKeys()
        {
            // Arrange.
            FakeResourceData();

            // Act.
            var actualResourceData = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResourceData.Keys);
        }

        [Fact]
        public void ReturnDataGotFromGetResourceDataByRefIdAndResourceIdOfRefDataHelper()
        {
            // Arrange.
            var expectedResourceData = FakeResourceData();

            // Act.
            var actualResourceData = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedResourceData, actualResourceData);
        }

        private Dictionary<string, string> FakeResourceData()
        {
            var expectedResourceData = new Dictionary<string, string>();

            foreach (var standardLanguageCode in StandardLanguageCodes)
            {
                var resourceInSpecificLanguage = Fixture.Create<string>();

                RefData
                    .GetResourceDataByRefIdAndResourceId(
                        Arg.Is(_refId),
                        Arg.Is(_resourceId),
                        Arg.Is(MappedRemotingRefLanguageCodes[standardLanguageCode]))
                    .Returns(resourceInSpecificLanguage);
                expectedResourceData.Add(standardLanguageCode, resourceInSpecificLanguage);
            }

            return expectedResourceData;
        }
    }
}