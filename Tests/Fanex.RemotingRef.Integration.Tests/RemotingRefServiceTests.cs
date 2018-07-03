namespace Fanex.RemotingRef.Integration.Tests
{
    using System;
    using System.Configuration;
    using Xunit;

    public class RemotingRefServiceTests
    {
        private const string DefaultValue = "defaultValue";
        private readonly IRemotingRefService _remotingRefService;
        private static readonly string _remotingRefServiceUrl = ConfigurationManager.AppSettings["RemotingRefServiceTest"];

        public RemotingRefServiceTests()
        {
            RefDataTypeContainer
                .ConnectTo(new Uri(_remotingRefServiceUrl))
                .FromApplication("UnitTest_AppName")
                .SupportOnlyDefaultLanguage();
            _remotingRefService = new RemotingRefService();
        }

        [Fact]
        public void GetBetTypeName_WithoutDefaultValue_RefResultHasValue_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US");

            // Assert
            Assert.Equal("Handicap", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1324234234", "en-US");

            // Assert
            Assert.Empty(betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_WithoutDefaultValue_RefResultHasValue_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US", 13242354353453);

            // Assert
            Assert.Equal("Handicap (Via Lucky)", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("2342342341", "en-US", 13242354353453);

            // Assert
            Assert.Empty(betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_WithDefaultValue_RefResultHasData_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US", 13242354353453, DefaultValue);

            // Assert
            Assert.Equal("Handicap (Via Lucky)", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("34242342341", "en-US", 13242354353453, DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceId_RefResultHasData_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("606", "en-US", DefaultValue, "10");

            // Assert
            Assert.Equal("1H Race To 10 Points", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceId_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("234234234606", "en-US", DefaultValue, "10");

            // Assert
            Assert.Equal(DefaultValue, betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceIdAndRefNo_RefResultHasData_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("606", "en-US", DefaultValue, "10", 13242354353453);

            // Assert
            Assert.Equal("1H Race To 10 Points (Via Lucky)", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceIdAndRefNo_RefResultIsNull_ReturnExpectedBetTypeName()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("60623423423424", "en-US", DefaultValue, "10", 13242354353453);

            // Assert
            Assert.Equal(DefaultValue, betTypeName);
        }

        [Fact]
        public void GetSportName_WithoutDefaultValue_RefResultHasData_ReturnExpectedSportName()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("1", "en-US");

            // Assert
            Assert.Equal("Soccer", sportName);
        }

        [Fact]
        public void GetSportName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("134242342", "en-US");

            // Assert
            Assert.Empty(sportName);
        }

        [Fact]
        public void GetSportName_WithDefaultValue_RefResultHasData_ReturnExpectedSportName()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Soccer", sportName);
        }

        [Fact]
        public void GetSportName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("134242342", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, sportName);
        }

        [Fact]
        public void GetTeamName_WithoutDefaultValue_RefResultHasData_ReturnExpectedTeamName()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("6", "en-US");

            // Assert
            Assert.Equal("Arsenal", teamName);
        }

        [Fact]
        public void GetTeamName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("234234426", "en-US");

            // Assert
            Assert.Empty(teamName);
        }

        [Fact]
        public void GetTeamName_WithDefaultValue_RefResultHasData_ReturnExpectedTeamName()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("6", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Arsenal", teamName);
        }

        [Fact]
        public void GetTeamName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("234234426", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, teamName);
        }

        [Fact]
        public void GetTeamName3rd_WithoutDefaultValue_RefResultHasData_ReturnExpectedTeamName()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName3rd("1", "en-US");

            // Assert
            Assert.Equal("Langsad SriHaYuth (DanYai)", teamName);
        }

        [Fact]
        public void GetTeamName3rd_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName3rd("1324234234", "en-US");

            // Assert
            Assert.Empty(teamName);
        }

        [Fact]
        public void GetTeamName3rd_WithDefaultValue_RefResultHasData_ReturnExpectedTeamName()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName3rd("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Langsad SriHaYuth (DanYai)", teamName);
        }

        [Fact]
        public void GetTeamName3rd_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName3rd("1324234234", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, teamName);
        }

        [Fact]
        public void GetLeagueName_WithoutDefaultValue_RefResultHasData_ReturnExpectedLeagueName()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("13", "en-US");

            // Assert
            Assert.Equal("UEFA CHAMPIONS LEAGUE QUALIFIERS", leagueName);
        }

        [Fact]
        public void GetLeagueName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("1324234234323", "en-US");

            // Assert
            Assert.Empty(leagueName);
        }

        [Fact]
        public void GetLeagueName_WithDefaultValue_RefResultHasData_ReturnExpectedLeagueName()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("13", "en-US", DefaultValue);

            // Assert
            Assert.Equal("UEFA CHAMPIONS LEAGUE QUALIFIERS", leagueName);
        }

        [Fact]
        public void GetLeagueName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("1324234234323", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_WithoutDefaultValue_RefResultHasData_ReturnExpectedLeagueName()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName3rd("1", "en-US");

            // Assert
            Assert.Equal("KlongHae Songkhla Early", leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName3rd("1324234234", "en-US");

            // Assert
            Assert.Empty(leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_WithDefaultValue_RefResultHasData_ReturnExpectedLeagueName()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName3rd("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("KlongHae Songkhla Early", leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName3rd("1324234234", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, leagueName);
        }

        [Fact]
        public void GetCountryName_WithDefaultValue_RefResultHasData_ReturnExpectedCountryName()
        {
            // Act
            var countryName = _remotingRefService.GetCountryName("AUS", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Australia", countryName);
        }

        [Fact]
        public void GetCountryName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var countryName = _remotingRefService.GetCountryName("AUSSSSSSS", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, countryName);
        }

        [Fact]
        public void GetCountryName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var countryName = _remotingRefService.GetCountryName("AUSSSSSSS", "en-US");

            // Assert
            Assert.Empty(countryName);
        }

        [Fact]
        public void GetOddsTypeName_WithoutDefaultValue_RefResultHasData_ReturnExpectedOddsTypeName()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1", "en-US");

            // Assert
            Assert.Equal("Dec", oddsTypeName);
        }

        [Fact]
        public void GetOddsTypeName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1234234234", "en-US");

            // Assert
            Assert.Empty(oddsTypeName);
        }

        [Fact]
        public void GetOddsTypeName_WithDefaultValue_RefResultHasData_ReturnExpectedOddsTypeName()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Dec", oddsTypeName);
        }

        [Fact]
        public void GetOddsTypeName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1234234234", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, oddsTypeName);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId_RefResultHasData_ReturnExpectedResourceData()
        {
            // Act
            var resourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId("AG_SicBo", "41", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Big", resourceData);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var resourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId("AG_SicBo23423423423", "41", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, resourceData);
        }

        [Fact]
        public void GetParentIdByBetTypeId_RefResultHasData_ReturnExpectedParentBetTypeId()
        {
            // Act
            var parentBetTypeId = _remotingRefService.GetParentIdByBetTypeId("178", DefaultValue);

            // Assert
            Assert.Equal("18", parentBetTypeId);
        }

        [Fact]
        public void GetParentIdByBetTypeId_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var parentBetTypeId = _remotingRefService.GetParentIdByBetTypeId("1782342342", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, parentBetTypeId);
        }

        [Fact]
        public void GetCombinationTypeCombo_RefResultHasData_ReturnExpectedCombinationTypeList()
        {
            // Act
            var combinationTypeList = _remotingRefService.GetCombinationTypeCombo("110", "2", 5, 10, "en-US");

            // Assert
            Assert.Equal(new[] { "Trixie" }, combinationTypeList);
        }

        [Fact]
        public void GetCombinationTypeCombo_RefResultIsNull_ReturnNull()
        {
            // Act
            var combinationTypeList = _remotingRefService.GetCombinationTypeCombo("1103322323", "2", 5, 10, "en-US");

            // Assert
            Assert.Null(combinationTypeList);
        }

        [Fact]
        public void GetHorseTeamName_WithDefaultValue_RefResultHasData_ReturnExpectedHorseTeamName()
        {
            // Act
            var horseTeamName = _remotingRefService.GetHorseTeamName("53438", "en-US", DefaultValue);

            // Assert
            Assert.Equal("T Keddy", horseTeamName);
        }

        [Fact]
        public void GetHorseTeamName_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var horseTeamName = _remotingRefService.GetHorseTeamName("53438234234", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, horseTeamName);
        }

        [Fact]
        public void GetHorseTeamName_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var horseTeamName = _remotingRefService.GetHorseTeamName("53438234234", "en-US");

            // Assert
            Assert.Empty(horseTeamName);
        }

        [Fact]
        public void GetCasinoBetItem_WithDefaultValue_RefResultHasData_ReturnExpectedCasinoBetItem()
        {
            // Act
            var casinoBetItem = _remotingRefService.GetCasinoBetItem("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Banker", casinoBetItem);
        }

        [Fact]
        public void GetCasinoBetItem_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var casinoBetItem = _remotingRefService.GetCasinoBetItem("1234234223", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, casinoBetItem);
        }

        [Fact]
        public void GetCasinoBetItem_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var casinoBetItem = _remotingRefService.GetCasinoBetItem("1234234223", "en-US");

            // Assert
            Assert.Empty(casinoBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_WithoutDefaultValue_RefResultHasData_ReturnExpectedRouletteBetItem()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("2", "en-US");

            // Assert
            Assert.Equal("1", rouletteBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("23423423", "en-US");

            // Assert
            Assert.Empty(rouletteBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_WithDefaultValue_RefResultHasData_ReturnExpectedRouletteBetItem()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("2", "en-US", DefaultValue);

            // Assert
            Assert.Equal("1", rouletteBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("23423423", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, rouletteBetItem);
        }

        [Fact]
        public void GetKenoBetChoice_WithDefaultValue_RefResultHasData_ReturnExpectedKenoBetChoice()
        {
            // Act
            var kenoBetChoice = _remotingRefService.GetKenoBetChoice("2", "en-US", DefaultValue, out string kenoBetType);

            // Assert
            Assert.Equal("Small", kenoBetChoice);
            Assert.Equal("1", kenoBetType);
        }

        [Fact]
        public void GetKenoBetChoice_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var kenoBetChoice = _remotingRefService.GetKenoBetChoice(null, "en-US", DefaultValue, out string kenoBetType);

            // Assert
            Assert.Equal(DefaultValue, kenoBetChoice);
            Assert.Equal(DefaultValue, kenoBetType);
        }

        [Fact]
        public void GetKenoBetChoice_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var kenoBetChoice = _remotingRefService.GetKenoBetChoice(null, "en-US", out string kenoBetType);

            // Assert
            Assert.Empty(kenoBetChoice);
            Assert.Empty(kenoBetType);
        }

        [Fact]
        public void GetKenoBetType_WithoutDefaultValue_RefResultHasData_ReturnExpectedKenoBetTypeName()
        {
            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("1", "en-US");

            // Assert
            Assert.Equal("Big/Small", kenoBetType);
        }

        [Fact]
        public void GetKenoBetType_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("234234231", "en-US");

            // Assert
            Assert.Empty(kenoBetType);
        }

        [Fact]
        public void GetKenoBetType_WithDefaultValue_RefResultHasData_ReturnExpectedKenoBetTypeName()
        {
            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("1", "en-US", DefaultValue);

            // Assert
            Assert.Equal("Big/Small", kenoBetType);
        }

        [Fact]
        public void GetKenoBetType_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("234234231", "en-US", DefaultValue);

            // Assert
            Assert.Equal(DefaultValue, kenoBetType);
        }

        [Fact]
        public void GetFastMarketsChoice_RefResultHasData_ReturnExpectedFastMarketsChoice()
        {
            // Act
            var fastMarketsChoice = _remotingRefService.GetFastMarketsChoice("222", "2", 3, 4, "en-US");

            // Assert
            Assert.Equal("Corner - Yes (03:00-07:59)", fastMarketsChoice);
        }

        [Fact]
        public void GetFastMarketsChoice_RefResultIsEmpty_ReturnEmptyString()
        {
            // Act
            var fastMarketsChoice = _remotingRefService.GetFastMarketsChoice("22324234322", "234232", 3, 4, "en-US");

            // Assert
            Assert.Equal(" (03:00-03:59)", fastMarketsChoice);
        }

        [Fact]
        public void GetFastMarkets225Result_RefResultHasData_ReturnExpectedFastMarkets255Result()
        {
            // Act
            var fastMarkets225Result = _remotingRefService.GetFastMarkets225Result("1", "en-US");

            // Assert
            Assert.Equal("None", fastMarkets225Result);
        }

        [Fact]
        public void GetFastMarkets225Result_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var fastMarkets225Result = _remotingRefService.GetFastMarkets225Result("1234232342344234", "en-US");

            // Assert
            Assert.Empty(fastMarkets225Result);
        }

        [Fact]
        public void GetLotteryBetChoice_WithoutDefaultValue_RefResultHasData_ReturnExpectedLotteryBetChoice()
        {
            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("40", "en-US", out string lotteryBetType);

            // Assert
            Assert.Equal("Green", lotteryBetChoice);
            Assert.Equal("16", lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetChoice_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("23423440", "en-US", out string lotteryBetType);

            // Assert
            Assert.Empty(lotteryBetChoice);
            Assert.Empty(lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetChoice_WithDefaultValue_RefResultHasData_ReturnExpectedLotteryBetChoice()
        {
            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("40", "en-US", DefaultValue, out string lotteryBetType);

            // Assert
            Assert.Equal("Green", lotteryBetChoice);
            Assert.Equal("16", lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetChoice_WithDefaultValue_RefResultIsNull_ReturnDefaultValue()
        {
            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("23423440", "en-US", DefaultValue, out string lotteryBetType);

            // Assert
            Assert.Equal(DefaultValue, lotteryBetChoice);
            Assert.Equal(DefaultValue, lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetType_WithoutDefaultValue_RefResultHasData_ReturnExpectedLotteryBetTypeName()
        {
            // Act
            var lotteryBetType = _remotingRefService.GetLotteryBetType("1", "en-US");

            // Assert
            Assert.Equal("1st Digit Big/Small", lotteryBetType);
        }

        [Fact]
        public void GetLotteryBetType_WithoutDefaultValue_RefResultIsNull_ReturnEmptyString()
        {
            // Act
            var lotteryBetType = _remotingRefService.GetLotteryBetType("324234241", "en-US");

            // Assert
            Assert.Empty(lotteryBetType);
        }

        [Fact]
        public void GetBetRadarMatchCodeFormat_RefResultHasData_ReturnExpectedBetRadarMatchCode()
        {
            // Act
            var mactchCode = _remotingRefService.GetBetRadarMatchCodeFormat("1324520 {10}", "en-US");

            // Assert
            Assert.Equal("1324520 Match Day 3 Groups C&D", mactchCode);
        }

        [Fact]
        public void GetBetRadarMatchCodeFormat_NoMatchCodeData_ReturnMatchCode()
        {
            // Act
            var mactchCode = _remotingRefService.GetBetRadarMatchCodeFormat("1324520 {1234230}", "en-US");

            // Assert
            Assert.Equal("1324520 ", mactchCode);
        }
    }
}