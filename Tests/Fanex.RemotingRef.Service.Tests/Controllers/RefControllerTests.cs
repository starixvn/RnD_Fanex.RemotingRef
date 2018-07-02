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
        private readonly RefController _refController;

        public RefControllerTests()
        {
            _refData = Substitute.For<IRefDataHelper>();
            _refController = new RefController(_refData);
        }

        [Fact]
        public void BetTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en").Returns("handicap");
            _refData.GetBetTypeName("1", "jp").Returns("handicap");
            _refData.GetBetTypeName("1", "vn").Returns(string.Empty);

            // Act
            var actualResult = _refController.BetTypeName("1", new[] { "en-US", "vi-VN", "ja-JP" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithRefNo_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", 1000000000).Returns("handicap");

            // Act
            var actualResult = _refController.BetTypeNameWithRefNo("1", 1000000000, new[] { "en-US" });

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

            // Act
            var actualResult = _refController.BetTypeNameWithResourceId("1", "123", new[] { "en-US", "ja-JP" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicapJP" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithRefNoAndResourceId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", "123", 1000000000).Returns("handicap");

            // Act
            var actualResult = _refController.BetTypeNameWithRefNoAndResourceId("1", "123", 1000000000, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void SportName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetSportName("1", "en").Returns("Soccer");

            // Act
            var actualResult = _refController.SportName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Soccer" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetTeamName("1", "en").Returns("Manchester");

            // Act
            var actualResult = _refController.TeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetTeamName3rd("1", "en").Returns("Manchester");

            // Act
            var actualResult = _refController.TeamName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLeagueName("1", "en").Returns("Champion League");

            // Act
            var actualResult = _refController.LeagueName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName3rd_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLeagueName3rd("1", "en").Returns("Champion League");

            // Act
            var actualResult = _refController.LeagueName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CountryName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCountryName("1", "en").Returns("USA");

            // Act
            var actualResult = _refController.CountryName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "USA" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void OddsTypeName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetOddsTypeName("1", "en").Returns("DecimalOdds");

            // Act
            var actualResult = _refController.OddsTypeName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "DecimalOdds" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ResourceData_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetResourceDataByRefIdAndResourceId("1", "2", "en").Returns("RefData");

            // Act
            var actualResult = _refController.ResourceData("1", "2", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RefData" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetParentIdByBetTypeId("1").Returns("2");

            // Act
            var actualResult = _refController.ParentIdByBetTypeId("1");

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCombinationTypeCombo("1", "2", 3, 4, "en").Returns(new[] { "Trixie" });

            // Act
            var actualResult = _refController.CombinationTypeComboParlay("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string[]> { { "en-US", new[] { "Trixie" } } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void HorseTeamName_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetHorseTeamName("1", "en").Returns("HorseTeam");

            // Act
            var actualResult = _refController.HorseTeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "HorseTeam" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CasinoBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetCasinoBetItem("1", "en").Returns("CasinoItem");

            // Act
            var actualResult = _refController.CasinoBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "CasinoItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void RouletteBetItem_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetRouletteBetItem("1", "en").Returns("RouletteBetItem");

            // Act
            var actualResult = _refController.RouletteBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RouletteBetItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetKenoBetChoice("1", "en", out string kenoBetType).Returns("RouletteBetItem");

            // Act
            var actualResult = _refController.KenoBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("RouletteBetItem", kenoBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetType_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetKenoBetType("1", "en").Returns("BetTypeName");

            // Act
            var actualResult = _refController.KenoBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarketsChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetFastMarketsChoice("1", "2", 3, 4, "en").Returns("Choice1");

            // Act
            var actualResult = _refController.FastMarketsChoice("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Choice1" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarkets225Result_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetFastMarkets225Result("1", "en").Returns("Result");

            // Act
            var actualResult = _refController.FastMarkets225Result("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Result" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetChoice_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLotteryBetChoice("1", "en", out string lotteryBetType).Returns("LotteryChoice");

            // Act
            var actualResult = _refController.LotteryBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("LotteryChoice", lotteryBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetType_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetLotteryBetType("1", "en").Returns("BetTypeName");

            // Act
            var actualResult = _refController.LotteryBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetRadarMatchCodeFormat_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetBetRadarMatchCodeFormat("1", "en").Returns("BetRadarMatchCodeFormat");

            // Act
            var actualResult = _refController.BetRadarMatchCodeFormat("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetRadarMatchCodeFormat" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void DisplayedTypeId_Always_GetExpectedRefResult()
        {
            // Arrange
            _refData.GetDisplayedTypeId("1").Returns("2");

            // Act
            var actualResult = _refController.DisplayedTypeId("1");

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, actualResult);
        }
    }
}