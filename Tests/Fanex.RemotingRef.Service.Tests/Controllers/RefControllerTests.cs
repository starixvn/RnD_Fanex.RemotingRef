namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using NSubstitute;
    using Xunit;

    public class RefControllerTests
    {
        private readonly IRefDataHelper _refData;

        public RefControllerTests()
        {
            _refData = Substitute.For<IRefDataHelper>();
        }

        [Fact]
        public void BetTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en").Returns("handicap");
            _refData.GetBetTypeName("1", "jp").Returns("handicap");
            _refData.GetBetTypeName("1", "vn").Returns(string.Empty);
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeName("1", new[] { "en-US", "vi-VN", "ja-JP" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithRefNo_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", 1000000000).Returns("handicap");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithRefNo("1", 1000000000, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithResourceId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", "123").Returns("handicap");
            _refData.GetBetTypeName("1", "jp", "123").Returns("handicapJP");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithResourceId("1", "123", new[] { "en-US", "ja-JP" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicapJP" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithRefNoAndResourceId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", "123", 1000000000).Returns("handicap");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithRefNoAndResourceId("1", "123", 1000000000, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void SportName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetSportName("1", "en").Returns("Soccer");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.SportName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Soccer" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetTeamName("1", "en").Returns("Manchester");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.TeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetTeamName3rd("1", "en").Returns("Manchester");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.TeamName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLeagueName("1", "en").Returns("Champion League");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LeagueName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLeagueName3rd("1", "en").Returns("Champion League");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LeagueName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CountryName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCountryName("1", "en").Returns("USA");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CountryName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "USA" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void OddsTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetOddsTypeName("1", "en").Returns("DecimalOdds");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.OddsTypeName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "DecimalOdds" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ResourceData_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetResourceDataByRefIdAndResourceId("1", "2", "en").Returns("RefData");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.ResourceData("1", "2", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RefData" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetParentIdByBetTypeId("1").Returns("2");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.ParentIdByBetTypeId("1");

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCombinationTypeCombo("1", "2", 3, 4, "en").Returns(new[] { "Trixie" });
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CombinationTypeComboParlay("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string[]> { { "en-US", new[] { "Trixie" } } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void HorseTeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetHorseTeamName("1", "en").Returns("HorseTeam");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.HorseTeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "HorseTeam" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CasinoBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCasinoBetItem("1", "en").Returns("CasinoItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CasinoBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "CasinoItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void RouletteBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetRouletteBetItem("1", "en").Returns("RouletteBetItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.RouletteBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RouletteBetItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetKenoBetChoice("1", "en", out string kenoBetType).Returns("RouletteBetItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.KenoBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("RouletteBetItem", kenoBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetType_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetKenoBetType("1", "en").Returns("BetTypeName");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.KenoBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarketsChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetFastMarketsChoice("1", "2", 3, 4, "en").Returns("Choice1");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.FastMarketsChoice("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Choice1" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarkets225Result_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetFastMarkets225Result("1", "en").Returns("Result");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.FastMarkets225Result("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Result" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLotteryBetChoice("1", "en", out string lotteryBetType).Returns("LotteryChoice");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LotteryBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("LotteryChoice", lotteryBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetType_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLotteryBetType("1", "en").Returns("BetTypeName");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LotteryBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetRadarMatchCodeFormat_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetRadarMatchCodeFormat("1", "en").Returns("BetRadarMatchCodeFormat");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetRadarMatchCodeFormat("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetRadarMatchCodeFormat" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void DisplayedTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetDisplayedTypeId("1").Returns("2");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.DisplayedTypeId("1");

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, actualResult);
        }
    }
}