namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class RefSyncControllerTests
    {
        private readonly Mock<IRefDataHelper> _refDataMock;

        public RefSyncControllerTests()
        {
            _refDataMock = new Mock<IRefDataHelper>();
        }

        [Fact]
        public void BetTypeName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetBetTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(dv => dv == string.Empty)))
                .Returns("handicap");
            var refSyncController = new RefSyncController(_refDataMock.Object);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>> { new Dictionary<string, object> { { "betTypeId", "1" } } },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.BetTypeName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets.First(), new Dictionary<string, string> { { "en-US", "handicap" } } }
            };

            Assert.Equal(expectedData, objectResult.Value);
        }
    }
}