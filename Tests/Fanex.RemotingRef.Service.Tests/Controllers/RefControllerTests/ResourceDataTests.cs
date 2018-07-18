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
            FakeResourceData();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(StandardLanguageCodes, actualResult.Keys);
        }

        [Fact]
        public void ResultTakesDataReturnedByRefDataHelperAsValues()
        {
            // Arrange.
            var expectedResourceData = FakeResourceData();

            // Act.
            var actualResult = RefController.ResourceData(_refId, _resourceId, StandardLanguageCodes);

            // Assert.
            Assert.Equal(expectedResourceData, actualResult.Values);
        }

        private List<string> FakeResourceData()
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
    }
}