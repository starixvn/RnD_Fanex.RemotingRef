namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;
    using NSubstitute;
    using Xunit;

    public class RefSyncControllerTests
    {
        private readonly IRefDataHelper _refData;

        public RefSyncControllerTests()
        {
            _refData = Substitute.For<IRefDataHelper>();
        }

        [Fact]
        public void BetTypeName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", string.Empty).Returns("handicap");
            _refData.GetBetTypeName("1", "en", 10000000000, string.Empty).Returns("handicap_refNo");
            _refData.GetBetTypeName("1", "en", string.Empty, "12").Returns("handicap_resourceId");
            _refData.GetBetTypeName("1", "en", string.Empty, "12", 10000000000).Returns("handicap_resourceId_refNo");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "refNo", 10000000000 } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "resourceId", "12" } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "resourceId", "12" }, { "refNo", 10000000000 } }
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.BetTypeName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "handicap" } } },
                { requestSyncData.ParameterSets[1], new Dictionary<string, string> { { "en-US", "handicap_refNo" } } },
                { requestSyncData.ParameterSets[2], new Dictionary<string, string> { { "en-US", "handicap_resourceId" } } },
                { requestSyncData.ParameterSets[3], new Dictionary<string, string> { { "en-US", "handicap_resourceId_refNo" } } }
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void SportName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetSportName("1", "ch", string.Empty).Returns("sport");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "sportTypeId", "1" } },
                },
                Languages = new[] { "zh-TW" }
            };

            // Act
            var actualResult = refSyncController.SportName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-TW", "sport" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void TeamName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetTeamName("1", "cs", string.Empty).Returns("team");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "teamId", "1" } },
                },
                Languages = new[] { "zh-CN" }
            };

            // Act
            var actualResult = refSyncController.TeamName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-CN", "team" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LeagueName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetLeagueName("1", "jp", string.Empty).Returns("league");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "leagueId", "1" } },
                },
                Languages = new[] { "ja-JP" }
            };

            // Act
            var actualResult = refSyncController.LeagueName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "ja-JP", "league" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CountryName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetCountryName("1", "th", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "countryCode", "1" } },
                },
                Languages = new[] { "th-TH" }
            };

            // Act
            var actualResult = refSyncController.CountryName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "th-TH", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void OddsTypeName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetOddsTypeName("1", "ko", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "oddsTypeId", "1" } },
                },
                Languages = new[] { "ko-KR" }
            };

            // Act
            var actualResult = refSyncController.OddsTypeName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "ko-KR", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void ResourceData_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetResourceDataByRefIdAndResourceId("1", "12", "vn", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "refId", "1" },  { "resourceId", "12" } },
                },
                Languages = new[] { "vi-VN" }
            };

            // Act
            var actualResult = refSyncController.ResourceData(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "vi-VN", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetParentIdByBetTypeId("1", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } },
                },
            };

            // Act
            var actualResult = refSyncController.ParentIdByBetTypeId(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetCombinationTypeCombo("1", "2", 3, 4, "vn").Returns(new[] { "vietnam" });
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTeam", "1" },  { "betCheck", "2" },  { "handicap1", 3 },  { "handicap2", 4 } },
                },
                Languages = new[] { "vi-VN" }
            };

            // Act
            var actualResult = refSyncController.CombinationTypeComboParlay(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string[]>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string[]> { { "vi-VN", new[] { "vietnam" } } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void HorseTeamName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetHorseTeamName("1", "zhcn", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "codeId", "1" } },
                },
                Languages = new[] { "zh-Hans" }
            };

            // Act
            var actualResult = refSyncController.HorseTeamName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-Hans", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CasinoBetItem_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetCasinoBetItem("1", "en", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "itemId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.CasinoBetItem(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void RouletteBetItem_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetRouletteBetItem("1", "en", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "itemId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.RouletteBetItem(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void KenoBetType_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetKenoBetType("1", "en", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "kenoBetTypeId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.KenoBetType(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void KenoBetChoice_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetKenoBetChoice("1", "en", string.Empty, out string kenoBetTypeId).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "kenoBetChoiceId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.KenoBetChoice(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("vietnam", kenoBetTypeId) } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void FastMarketsChoice_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetFastMarketsChoice("1", "2", 3, 4, "en").Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" },  { "betTeam", "2" },  { "handicap1", 3 },  { "handicap2", 4 } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.FastMarketsChoice(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void FastMarkets225Result_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetFastMarkets225Result("1", "en").Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "resultId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.FastMarkets225Result(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LotteryBetChoice_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetLotteryBetChoice("1", "en", string.Empty, out string lotteryBetTypeId).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betChoiceId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.LotteryBetChoice(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("vietnam", lotteryBetTypeId) } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LotteryBetTypeName_Always_CallRefDataWithCorrectParameters()
        {
            // Arrange
            _refData.GetLotteryBetType("1", "en", string.Empty).Returns("vietnam");
            var refSyncController = new RefSyncController(_refData);
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } },
                },
                Languages = new[] { "en-US" }
            };

            // Act
            var actualResult = refSyncController.LotteryBetTypeName(requestSyncData);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "vietnam" } } },
            };

            Assert.Equal(expectedData, objectResult.Value);
        }
    }
}