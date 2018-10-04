namespace Fanex.RemotingRef.Integration.Tests
{
    using System.ComponentModel;
    using Xunit;

    public class HappyCasesTests : ServiceIntegrationTestBase
    {
        [Fact]
        public void GetBetTypeName()
        {
            // Arrange.
            var betTypeId = "2";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "Odd/Even";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetViaLuckyBetTypeName()
        {
            // Arrange.
            var betTypeId = "2";
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo);

            // Assert.
            var expectedBetTypeName = "Odd/Even (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("Get bet type name stored in DB, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetBetTypeNameContaingPlaceholders()
        {
            // Arrange.
            var betTypeId = "607";
            var resourceId = "01";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId);

            // Assert.
            var expectedBetTypeName = "2H Race To 1 Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("Get Via Lucky bet type name, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetViaLuckyBetTypeNameContainingPlaceholders()
        {
            // Arrange.
            var betTypeId = "607";
            var resourceId = "02";
            var refNo = 100_000_000_001L;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId,
                refNo);

            // Assert.
            var expectedBetTypeName = "2H Race To 2 Points (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetSportName()
        {
            // Arrange.
            var sportTypeId = "2";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguage);

            // Assert.
            var expectedSportName = "Basketball";

            Assert.Equal(expectedSportName, actualSportName);
        }

        [Fact]
        public void GetTeamName()
        {
            // Arrange.
            var teamId = "5";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguage);

            // Assert.
            var expectedTeamName = "Manchester United";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        public void GetTeamName3rd()
        {
            // Arrange.
            var teamId3rd = "2";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName3rd(teamId3rd, TestingLanguage);

            // Assert.
            var expectedTeamName = "Dukdang SamChay (KhaoNoi)";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        public void GetLeagueName()
        {
            // Arrange.
            var leagueId = "3";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguage);

            // Assert.
            var expectedLeagueName = "*ENGLISH PREMIER LEAGUE";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Fact]
        public void GetLeagueName3rd()
        {
            // Arrange.
            var leagueId3rd = "2";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            var expectedLeagueName = "KlongHae Songkhla";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Fact]
        public void GetCountryName()
        {
            // Arrange.
            var countryCode = "KOR";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguage);

            // Assert.
            var expectedCountryName = "Korea (Rep)";

            Assert.Equal(expectedCountryName, actualCountryName);
        }

        [Fact]
        public void GetOddsTypeName()
        {
            // Arrange.
            var oddsTypeId = "2";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguage);

            // Assert.
            var expectedOddsTypeName = "HK";

            Assert.Equal(expectedOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId()
        {
            // Arrange.
            var refId = "AG_Baccarat";
            var resourceId = "1";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedResource = "Tie";

            Assert.Equal(expectedResource, actualResource);
        }

        [Fact]
        public void GetParentIdByBetTypeId()
        {
            // Arrange.
            var betTypeId = "157";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            var expectedParentBetTypeId = "2";

            Assert.Equal(expectedParentBetTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void GetCombinationTypeCombo()
        {
            // Arrange.
            var betTeam = "120";
            var betCheck = "4";
            var handicap1 = 1;
            var handicap2 = 3;

            // Act.
            var actualCombos = RemotingRefService.GetCombinationTypeCombo(
                betTeam,
                betCheck,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedCombos = new[] { "Treble (1 Bet)" };

            Assert.Equal(expectedCombos, actualCombos);
        }

        [Fact]
        public void GetHorseTeamName()
        {
            // Arrange.
            var codeId = "33231";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguage);

            // Assert.
            var expectedHorseTeamName = "SUN KING";

            Assert.Equal(expectedHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void GetCasinoBetItem()
        {
            // Arrange.
            var itemId = "1";

            // Act.
            var actualBetItem = RemotingRefService.GetCasinoBetItem(itemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "Banker";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        public void GetRouletteBetItem()
        {
            // Arrange.
            var itemId = "2";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguage);

            // Assert.
            var expectedBetItem = "1";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        public void GetKenoBetChoice()
        {
            // Arrange.
            var kenoBetChoiceId = "1";

            // Act.
            var actualBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedBetChoice = "Big";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        public void GetKenoBetTypeIdFromBetChoiceId()
        {
            // Arrange.
            var kenoBetChoiceId = "1";

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            var expectedBetTypeId = "1";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Fact]
        public void GetKenoBetType()
        {
            // Arrange.
            var kenoBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "Big/Small";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetFastMarketsChoice()
        {
            // Arrange.
            var betTypeId = "222";
            var betTeam = "2";
            var handicap1 = 3;
            var handicap2 = 4;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            var expectedChoice = "Corner - Yes (03:00-07:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetFastMarketsResult()
        {
            // Arrange.
            var resultId = "3";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguage);

            // Assert.
            var expectedResult = "Goal, None";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetFastMarkets225Result()
        {
            // Arrange.
            var betType255ResultId = "163";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguage);

            // Assert.
            var expectedResult = "Penalty, Throw In, None";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetLotteryBetChoice()
        {
            // Arrange.
            var lotteryBetChoiceId = "40";

            // Act.
            var actualBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            var expectedBetChoice = "Green";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        public void GetLotteryBetTypeIdFromBetChoiceId()
        {
            // Arrange.
            var lotteryBetChoiceId = "40";

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            var expectedBetTypeId = "16";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Fact]
        public void GetLotteryBetType()
        {
            // Arrange.
            var lotteryBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            var expectedBetTypeName = "1st Digit Big/Small";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetRadarMatchCodeFormat()
        {
            // Arrange.
            var matchCode = "101090 {10}";

            // Act.
            var actualMatch = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguage);

            // Assert.
            var expectedMatch = "101090 Match Day 3 Groups C&D";

            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        public void GetDisplayedTypeId()
        {
            // Arrange.
            var betTypeId = "157";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            var expectedBetTypeId = "2";

            Assert.Equal(expectedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void GetDisplayedBetTypeName()
        {
            // Arrange.
            var betTypeId = "407";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Home Team Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void GetDisplayedViaLuckyBetTypeName()
        {
            // Arrange.
            var betTypeId = "407";
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage);

            // Assert.
            var expectedDisplayedBetTypeName = "Exact Home Team Goals (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("Get bet type name stored in DB, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetDisplayedBetTypeNameContaingPlaceholders()
        {
            // Arrange.
            var betTypeId = "1316";
            var resourceId = "01";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 1 Game Handicap";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("Get Via Lucky bet type name, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetDisplayedViaLuckyBetTypeNameContainingPlaceholders()
        {
            // Arrange.
            var betTypeId = "1316";
            var resourceId = "02";
            var refNo = 100_000_000_001L;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            var expectedDisplayedBetTypeName = "Set 2 Game Handicap (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }
    }
}