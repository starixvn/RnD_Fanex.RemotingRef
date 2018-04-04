namespace Fanex.RemotingRef.Integration.Tests
{
    using System;
    using System.Configuration;
    using Xunit;

    public class RemotingRefServiceTests
    {
        private readonly IRemotingRefService _remotingRefService;
        private static readonly string _remotingRefServiceUrl = ConfigurationManager.AppSettings["RemotingRefServiceTest"];

        public RemotingRefServiceTests()
        {
            RefDataTypeContainer.ConnectTo(new Uri(_remotingRefServiceUrl)).SupportOnlyDefaultLanguage();
            _remotingRefService = new RemotingRefService();
        }

        [Fact]
        public void GetBetTypeName_Always_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US");

            // Assert
            Assert.Equal("Handicap", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US", 13242354353453);

            // Assert
            Assert.Equal("Handicap (Via Lucky)", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceId_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("606", "en-US", string.Empty, "10");

            // Assert
            Assert.Equal("1H Race To 10 Points", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceIdAndRefNo_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("606", "en-US", string.Empty, "10", 13242354353453);

            // Assert
            Assert.Equal("1H Race To 10 Points (Via Lucky)", betTypeName);
        }

        [Fact]
        public void GetSportName_Always_ReturnExpectedSportName()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("1", "en-US");

            // Assert
            Assert.Equal("Soccer", sportName);
        }

        [Fact]
        public void GetTeamName_Always_ReturnExpectedTeamName()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("6", "en-US");

            // Assert
            Assert.Equal("Arsenal", teamName);
        }

        [Fact]
        public void GetTeamName3rd_Always_ReturnExpectedTeamName()
        {
            // TODO: implement later
        }

        [Fact]
        public void GetLeagueName_Always_ReturnExpectedLeagueName()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("13", "en-US");

            // Assert
            Assert.Equal("UEFA CHAMPIONS LEAGUE QUALIFIERS", leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_Always_ReturnExpectedTeamName()
        {
            // TODO: implement later
        }

        [Fact]
        public void GetCountryName_Always_ReturnExpectedCountryName()
        {
            // Act
            var countryName = _remotingRefService.GetCountryName("AUS", "en-US", string.Empty);

            // Assert
            Assert.Equal("Australia", countryName);
        }

        [Fact]
        public void GetOddsTypeName_Always_ReturnExpectedOddsTypeName()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1", "en-US", string.Empty);

            // Assert
            Assert.Equal("Dec", oddsTypeName);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId_Always_ReturnExpectedResourceData()
        {
            // Act
            var resourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId("AG_SicBo", "41", "en-US", string.Empty);

            // Assert
            Assert.Equal("Big", resourceData);
        }

        [Fact]
        public void GetParentIdByBetTypeId_Always_ReturnExpectedParentBetTypeId()
        {
            // Act
            var parentBetTypeId = _remotingRefService.GetParentIdByBetTypeId("178", string.Empty);

            // Assert
            Assert.Equal("18", parentBetTypeId);
        }

        [Fact]
        public void GetCombinationTypeCombo_Always_ReturnExpectedCombinationTypeList()
        {
            // Act
            var combinationTypeList = _remotingRefService.GetCombinationTypeCombo("110", "2", 5, 10, string.Empty);

            // Assert
            Assert.Equal(new[] { "Trixie" }, combinationTypeList);
        }

        [Fact]
        public void GetCasinoBetItem_Always_ReturnExpectedCasinoBetItem()
        {
            // Act
            var casinoBetItem = _remotingRefService.GetCasinoBetItem("1", "en-US", string.Empty);

            // Assert
            Assert.Equal("Banker", casinoBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_Always_ReturnExpectedRouletteBetItem()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("2", "en-US", string.Empty);

            // Assert
            Assert.Equal("1", rouletteBetItem);
        }

        [Fact]
        public void GetKenoBetChoice_Always_ReturnExpectedKenoBetChoice()
        {
            // Act
            var kenoBetChoice = _remotingRefService.GetKenoBetChoice("2", "en-US", string.Empty, out string kenoBetType);

            // Assert
            Assert.Equal("Small", kenoBetChoice);
            Assert.Equal("1", kenoBetType);
        }

        [Fact]
        public void GetKenoBetType_Always_ReturnExpectedKenoBetTypeName()
        {
            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("1", "en-US", string.Empty);

            // Assert
            Assert.Equal("Big/Small", kenoBetType);
        }

        [Fact]
        public void GetFastMarketsChoice_Always_ReturnExpectedFastMarketsChoice()
        {
            // Act
            var fastMarketsChoice = _remotingRefService.GetFastMarketsChoice("222", "2", 3, 4, "en-US");

            // Assert
            Assert.Equal("Corner-Yes (03:00-07:59)", fastMarketsChoice);
        }

        [Fact]
        public void GetFastMarkets225Result_Always_ReturnExpectedFastMarkets255Result()
        {
            // Act
            var fastMarkets225Result = _remotingRefService.GetFastMarkets225Result("1", "en-US");

            // Assert
            Assert.Equal("None", fastMarkets225Result);
        }

        [Fact]
        public void GetLotteryBetChoice_Always_ReturnExpectedLotteryBetChoice()
        {
            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("40", "en-US", string.Empty, out string lotteryBetType);

            // Assert
            Assert.Equal("Green", lotteryBetChoice);
            Assert.Equal("16", lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetType_Always_ReturnExpectedLotteryBetTypeName()
        {
            // Act
            var lotteryBetType = _remotingRefService.GetLotteryBetType("1", "en-US", string.Empty);

            // Assert
            Assert.Equal("1st Digit Big/Small", lotteryBetType);
        }
    }
}