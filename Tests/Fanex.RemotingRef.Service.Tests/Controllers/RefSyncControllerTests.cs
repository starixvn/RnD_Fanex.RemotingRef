namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using NSubstitute;
    using Xunit;

    public class RefSyncControllerTests
    {
        private readonly IRefDataHelper _refData;
        private readonly RefSyncController _refSyncController;

        public RefSyncControllerTests()
        {
            _refData = Substitute.For<IRefDataHelper>();
            _refSyncController = new RefSyncController(_refData);
        }

        [Fact]
        public void BetTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "refNo", 10000000000 } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "resourceId", "12" } },
                    new Dictionary<string, object> { { "betTypeId", "1" }, { "resourceId", "12" }, { "refNo", 10000000000 } },
                    new Dictionary<string, object> { { "betTypeId", "2" }, { "resourceId", "12" }, { "refNo", 10000000000 } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetBetTypeName("1", "en").Returns("handicap");
            _refData.GetBetTypeName("1", "en", 10000000000).Returns("handicap_refNo");
            _refData.GetBetTypeName("1", "en", "12").Returns("handicap_resourceId");
            _refData.GetBetTypeName("1", "en", "12", 10000000000).Returns("handicap_resourceId_refNo");
            _refData.GetBetTypeName("2", "en", "12", 10000000000).Returns(string.Empty);

            // Act
            var actualResult = _refSyncController.BetTypeName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "handicap" } } },
                { requestSyncData.ParameterSets[1], new Dictionary<string, string> { { "en-US", "handicap_refNo" } } },
                { requestSyncData.ParameterSets[2], new Dictionary<string, string> { { "en-US", "handicap_resourceId" } } },
                { requestSyncData.ParameterSets[3], new Dictionary<string, string> { { "en-US", "handicap_resourceId_refNo" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void SportName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "sportTypeId", "1" } }
                },
                Languages = new[] { "zh-TW" }
            };
            _refData.GetSportName("1", "ch").Returns("sport");

            // Act
            var actualResult = _refSyncController.SportName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-TW", "sport" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "teamId", "1" } },
                    new Dictionary<string, object> { { "teamId", "2" } }
                },
                Languages = new[] { "zh-CN" }
            };
            _refData.GetTeamName("1", "cs").Returns("team");
            _refData.GetTeamName("2", "cs").Returns(string.Empty);

            // Act
            var actualResult = _refSyncController.TeamName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-CN", "team" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "team3rdId", "1" } },
                    new Dictionary<string, object> { { "team3rdId", "2" } }
                },
                Languages = new[] { "zh-CN" }
            };
            _refData.GetTeamName3rd("1", "cs").Returns("team");
            _refData.GetTeamName3rd("2", "cs").Returns(string.Empty);

            // Act
            var actualResult = _refSyncController.TeamName3rd(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-CN", "team" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "leagueId", "1" } }
                },
                Languages = new[] { "ja-JP" }
            };
            _refData.GetLeagueName("1", "jp").Returns("league");

            // Act
            var actualResult = _refSyncController.LeagueName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "ja-JP", "league" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "league3rdId", "1" } }
                },
                Languages = new[] { "ja-JP" }
            };
            _refData.GetLeagueName3rd("1", "jp").Returns("league");

            // Act
            var actualResult = _refSyncController.LeagueName3rd(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "ja-JP", "league" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CountryName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "countryCode", "1" } }
                },
                Languages = new[] { "th-TH" }
            };
            _refData.GetCountryName("1", "th").Returns("vietnam");

            // Act
            var actualResult = _refSyncController.CountryName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "th-TH", "vietnam" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void OddsTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "oddsTypeId", "1" } }
                },
                Languages = new[] { "ko-KR" }
            };
            _refData.GetOddsTypeName("1", "ko").Returns("decimal");

            // Act
            var actualResult = _refSyncController.OddsTypeName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "ko-KR", "decimal" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ResourceData_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "refId", "1" },  { "resourceId", "12" } }
                },
                Languages = new[] { "vi-VN" }
            };
            _refData.GetResourceDataByRefIdAndResourceId("1", "12", "vn").Returns("casino");

            // Act
            var actualResult = _refSyncController.ResourceData(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "vi-VN", "casino" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } }
                },
            };
            _refData.GetParentIdByBetTypeId("1").Returns("2");

            // Act
            var actualResult = _refSyncController.ParentIdByBetTypeId(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "2" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTeam", "1" },  { "betCheck", "2" },  { "handicap1", 3 },  { "handicap2", 4 } },
                    new Dictionary<string, object> { { "betTeam", "2" },  { "betCheck", "2" },  { "handicap1", 3 },  { "handicap2", 4 } },
                    new Dictionary<string, object> { { "betTeam", "3" },  { "betCheck", "2" },  { "handicap1", 3 },  { "handicap2", 4 } }
                },
                Languages = new[] { "vi-VN" }
            };
            _refData.GetCombinationTypeCombo("1", "2", 3, 4, "vn").Returns(new[] { "trixie" });
            _refData.GetCombinationTypeCombo("2", "2", 3, 4, "vn").Returns(new string[] { });
            _refData.GetCombinationTypeCombo("3", "2", 3, 4, "vn").Returns(new[] { string.Empty });

            // Act
            var actualResult = _refSyncController.CombinationTypeComboParlay(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string[]>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string[]> { { "vi-VN", new[] { "trixie" } } } },
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void HorseTeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "codeId", "1" } }
                },
                Languages = new[] { "zh-Hans" }
            };
            _refData.GetHorseTeamName("1", "zhcn").Returns("horse");

            // Act
            var actualResult = _refSyncController.HorseTeamName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "zh-Hans", "horse" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CasinoBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "itemId", "1" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetCasinoBetItem("1", "en").Returns("casino");

            // Act
            var actualResult = _refSyncController.CasinoBetItem(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "casino" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void RouletteBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "itemId", "1" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetRouletteBetItem("1", "en").Returns("roulette");

            // Act
            var actualResult = _refSyncController.RouletteBetItem(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "roulette" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetType_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "kenoBetTypeId", "1" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetKenoBetType("1", "en").Returns("keno");

            // Act
            var actualResult = _refSyncController.KenoBetType(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "keno" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "kenoBetChoiceId", "1" } },
                    new Dictionary<string, object> { { "kenoBetChoiceId", "2" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetKenoBetChoice("1", "en", out string kenoBetTypeId).Returns("keno");
            _refData.GetKenoBetChoice("2", "en", out string kenoBetTypeIdTwo).Returns(string.Empty);

            // Act
            var actualResult = _refSyncController.KenoBetChoice(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("keno", kenoBetTypeId) } } },
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarketsChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" },  { "betTeam", "2" },  { "handicap1", 3 },  { "handicap2", 4 } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetFastMarketsChoice("1", "2", 3, 4, "en").Returns("choice1");

            // Act
            var actualResult = _refSyncController.FastMarketsChoice(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "choice1" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarkets225Result_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "resultId", "1" } },
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetFastMarkets225Result("1", "en").Returns("goal");

            // Act
            var actualResult = _refSyncController.FastMarkets225Result(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "goal" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betChoiceId", "1" } },
                    new Dictionary<string, object> { { "betChoiceId", "2" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetLotteryBetChoice("1", "en", out string lotteryBetTypeId).Returns("choice");
            _refData.GetLotteryBetChoice("2", "en", out string lotteryBetTypeIdTwo).Returns(string.Empty);

            // Act
            var actualResult = _refSyncController.LotteryBetChoice(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, Tuple<string, string>>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("choice", lotteryBetTypeId) } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetLotteryBetType("1", "en").Returns("bettype");

            // Act
            var actualResult = _refSyncController.LotteryBetTypeName(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "bettype" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetRadarMatchCodeFormat_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "matchCode", "1" } }
                },
                Languages = new[] { "en-US" }
            };
            _refData.GetBetRadarMatchCodeFormat("1", "en").Returns("BetRadarMatchCodeFormat");

            // Act
            var actualResult = _refSyncController.BetRadarMatchCodeFormat(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "BetRadarMatchCodeFormat" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void DisplayedTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            var requestSyncData = new RequestSyncFormData
            {
                ParameterSets = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { { "betTypeId", "1" } }
                },
            };
            _refData.GetDisplayedTypeId("1").Returns("2");

            // Act
            var actualResult = _refSyncController.DisplayedTypeId(requestSyncData);

            // Assert
            var expectedData = new Dictionary<Dictionary<string, object>, Dictionary<string, string>>
            {
                { requestSyncData.ParameterSets[0], new Dictionary<string, string> { { "en-US", "2" } } }
            };

            Assert.Equal(expectedData, actualResult);
        }
    }
}