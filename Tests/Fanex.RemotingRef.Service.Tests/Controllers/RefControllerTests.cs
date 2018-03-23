namespace Fanex.RemotingRef.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Fanex.RemotingRef.Service.MVC.Controllers;
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
        public void BetTypeName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", string.Empty).Returns("handicap");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithRefNo_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", 1000000000, string.Empty).Returns("handicap");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithRefNo("1", 1000000000, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void BetTypeNameWithResourceId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", string.Empty, "123").Returns("handicap");
            _refData.GetBetTypeName("1", "jp", string.Empty, "123").Returns("handicapJP");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithResourceId("1", "123", new[] { "en-US", "ja-JP" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" }, { "ja-JP", "handicapJP" } };
            Assert.Equal(expectedData, expectedData);
        }

        [Fact]
        public void BetTypeNameWithRefNoAndResourceId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetBetTypeName("1", "en", string.Empty, "123", 1000000000).Returns("handicap");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.BetTypeNameWithRefNoAndResourceId("1", "123", 1000000000, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "handicap" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void SportName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetSportName("1", "en", string.Empty).Returns("Soccer");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.SportName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Soccer" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetTeamName("1", "en", string.Empty)
                .Returns("Manchester");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.TeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void TeamName3rd_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetTeamName3rd("1", "en", string.Empty).Returns("Manchester");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.TeamName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Manchester" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetLeagueName("1", "en", string.Empty)
                .Returns("Champion League");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LeagueName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LeagueName3rd_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetLeagueName3rd("1", "en", string.Empty)
                .Returns("Champion League");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LeagueName3rd("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Champion League" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CountryName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetCountryName("1", "en", string.Empty)
                .Returns("USA");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CountryName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "USA" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void OddsTypeName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetOddsTypeName("1", "en", string.Empty)
                .Returns("DecimalOdds");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.OddsTypeName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "DecimalOdds" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ResourceData_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetResourceDataByRefIdAndResourceId("1", "2", "en", string.Empty)
                .Returns("RefData");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.ResourceData("1", "2", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RefData" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void ParentIdByBetTypeId_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetParentIdByBetTypeId("1", string.Empty)
                .Returns("2");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.ParentIdByBetTypeId("1");

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "2" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CombinationTypeComboParlay_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetCombinationTypeCombo("1", "2", 3, 4, "en")
                .Returns(new[] { "Trixie" });
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CombinationTypeComboParlay("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string[]> { { "en-US", new[] { "Trixie" } } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void HorseTeamName_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetHorseTeamName("1", "en", string.Empty)
                .Returns("HorseTeam");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.HorseTeamName("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "HorseTeam" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void CasinoBetItem_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetCasinoBetItem("1", "en", string.Empty)
                .Returns("CasinoItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.CasinoBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "CasinoItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void RouletteBetItem_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetRouletteBetItem("1", "en", string.Empty)
                .Returns("RouletteBetItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.RouletteBetItem("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "RouletteBetItem" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetKenoBetChoice("1", "en", string.Empty, out string kenoBetType)
                .Returns("RouletteBetItem");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.KenoBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("RouletteBetItem", kenoBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void KenoBetType_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetKenoBetType("1", "en", string.Empty)
                .Returns("BetTypeName");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.KenoBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarketsChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetFastMarketsChoice("1", "2", 3, 4, "en")
                .Returns("Choice1");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.FastMarketsChoice("1", "2", 3, 4, new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Choice1" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void FastMarkets225Result_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetFastMarkets225Result("1", "en")
                .Returns("Result");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.FastMarkets225Result("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "Result" } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetChoice_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetLotteryBetChoice("1", "en", string.Empty, out string lotteryBetType)
                .Returns("LotteryChoice");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LotteryBetChoice("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("LotteryChoice", lotteryBetType) } };
            Assert.Equal(expectedData, actualResult);
        }

        [Fact]
        public void LotteryBetType_Always_CallRefDataWithCorrectParameter()
        {
            // Arrange
            _refData.GetLotteryBetType("1", "en", string.Empty)
                .Returns("BetTypeName");
            var refController = new RefController(_refData);

            // Act
            var actualResult = refController.LotteryBetType("1", new[] { "en-US" });

            // Assert
            var expectedData = new Dictionary<string, string> { { "en-US", "BetTypeName" } };
            Assert.Equal(expectedData, actualResult);
        }
    }
}