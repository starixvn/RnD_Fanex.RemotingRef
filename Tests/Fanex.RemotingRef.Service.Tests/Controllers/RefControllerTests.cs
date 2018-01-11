namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.Controllers;
    using Fanex.RemotingRef.Service.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class RefControllerTests
    {
        private readonly Mock<IRefDataHelper> _refDataMock;

        public RefControllerTests()
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
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.BetTypeName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void BetTypeNameWithRefNo_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetBetTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<long>(refNo => refNo == 1000000000),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("handicap");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.BetTypeName("1", 1000000000, new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void BetTypeNameWithResourceId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetBetTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty),
                    It.Is<string>(resourceId => resourceId == "123")))
                .Returns("handicap");
            _refDataMock.Setup(r => r.GetBetTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "jp"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty),
                    It.Is<string>(resourceId => resourceId == "123")))
                .Returns("handicapJP");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.BetTypeName("1", "123", new[] { "en-US", "ja-JP" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicapJP" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void BetTypeNameWithRefNoAndResourceId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetBetTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty),
                    It.Is<string>(resourceId => resourceId == "123"),
                     It.Is<long>(refNo => refNo == 1000000000)))
                .Returns("handicap");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.BetTypeName("1", "123", 1000000000, new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void SportName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetSportName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("Soccer");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.SportName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Soccer" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void TeamName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetTeamName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("Manchester");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.TeamName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void TeamName3rd_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetTeamName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("Manchester");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.TeamName3rd("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LeagueName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetLeagueName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("Champion League");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.LeagueName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LeagueName3rd_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetLeagueName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("Champion League");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.LeagueName3rd("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CountryName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetCountryName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("USA");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.CountryName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "USA" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void OddsTypeName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetOddsTypeName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("DecimalOdds");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.OddsTypeName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "DecimalOdds" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void ResourceData_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetResourceDataByRefIdAndResourceId(
                    It.Is<string>(refId => refId == "1"),
                    It.Is<string>(resourceId => resourceId == "2"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("RefData");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.ResourceData("1", "2", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "RefData" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetParentIdByBetTypeId(
                    It.Is<string>(parentId => parentId == "1"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("2");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.ParentIdByBetTypeId("1");

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetCombinationTypeCombo(
                    It.Is<string>(betTeam => betTeam == "1"),
                    It.Is<string>(betCheck => betCheck == "2"),
                    It.Is<int>(handicap1 => handicap1 == 3),
                    It.Is<int>(handicap2 => handicap2 == 4),
                    It.Is<string>(lang => lang == "en")))
                .Returns(new[] { "Trixie" });
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.CombinationTypeComboParlay("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string[]> { { "en-US", new[] { "Trixie" } } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void HorseTeamName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetHorseTeamName(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("HorseTeam");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.HorseTeamName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "HorseTeam" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void CasinoBetItem_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetCasinoBetItem(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("CasinoItem");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.CasinoBetItem("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "CasinoItem" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void RouletteBetItem_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetRouletteBetItem(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("RouletteBetItem");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.RouletteBetItem("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "RouletteBetItem" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void KenoBetChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            var kenoBetType = "100";
            _refDataMock.Setup(r => r.GetKenoBetChoice(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty),
                    out kenoBetType))
                .Returns("RouletteBetItem");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.KenoBetChoice("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("RouletteBetItem", "100") } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void KenoBetType_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetKenoBetType(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("BetTypeName");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.KenoBetType("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void FastMarketsChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetFastMarketsChoice(
                    It.Is<string>(betTeam => betTeam == "1"),
                    It.Is<string>(betCheck => betCheck == "2"),
                    It.Is<int>(handicap1 => handicap1 == 3),
                    It.Is<int>(handicap2 => handicap2 == 4),
                    It.Is<string>(lang => lang == "en")))
                .Returns("Choice1");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.FastMarketsChoice("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Choice1" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void FastMarkets225Result_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetFastMarkets225Result(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en")))
                .Returns("Result");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.FastMarkets225Result("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "Result" } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LotteryBetChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            var lotteryBetType = "100";
            _refDataMock.Setup(r => r.GetLotteryBetChoice(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty),
                    out lotteryBetType))
                .Returns("LotteryChoice");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.LotteryBetChoice("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("LotteryChoice", "100") } };
            Assert.Equal(expectedData, objectResult.Value);
        }

        [Fact]
        public void LotteryBetTypeName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refDataMock.Setup(r => r.GetLotteryBetType(
                    It.Is<string>(id => id == "1"),
                    It.Is<string>(lang => lang == "en"),
                    It.Is<string>(defaultValue => defaultValue == string.Empty)))
                .Returns("BetTypeName");
            var refController = new RefController(_refDataMock.Object);

            // Act
            var actualResult = refController.LotteryBetTypeName("1", new[] { "en-US" });

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(actualResult);
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, objectResult.Value);
        }
    }
}