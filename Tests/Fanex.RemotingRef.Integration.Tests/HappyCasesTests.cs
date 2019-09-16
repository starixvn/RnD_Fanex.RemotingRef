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
            const string betTypeId = "2";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            const string expectedBetTypeName = "Odd/Even";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetViaLuckyBetTypeName()
        {
            // Arrange.
            const string betTypeId = "2";
            const long refNo = 100_000_000_001;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                refNo);

            // Assert.
            const string expectedBetTypeName = "Odd/Even (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("Get bet type name stored in DB, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetBetTypeNameContaingPlaceholders()
        {
            // Arrange.
            const string betTypeId = "607";
            const string resourceId = "01";

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId);

            // Assert.
            const string expectedBetTypeName = "2H Race To 1 Points";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        [Description("Get Via Lucky bet type name, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetViaLuckyBetTypeNameContainingPlaceholders()
        {
            // Arrange.
            const string betTypeId = "607";
            const string resourceId = "02";
            const long refNo = 100_000_000_001;

            // Act.
            var actualBetTypeName = RemotingRefService.GetBetTypeName(
                betTypeId,
                TestingLanguage,
                DefaultValue,
                resourceId,
                refNo);

            // Assert.
            const string expectedBetTypeName = "2H Race To 2 Points (Via Lucky)";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetSportName()
        {
            // Arrange.
            const string sportTypeId = "2";

            // Act.
            var actualSportName = RemotingRefService.GetSportName(sportTypeId, TestingLanguage);

            // Assert.
            const string expectedSportName = "Basketball";

            Assert.Equal(expectedSportName, actualSportName);
        }

        [Fact]
        public void GetTeamName()
        {
            // Arrange.
            const string teamId = "5";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName(teamId, TestingLanguage);

            // Assert.
            const string expectedTeamName = "Manchester United";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        public void GetTeamName3rd()
        {
            // Arrange.
            const string teamId3rd = "2";

            // Act.
            var actualTeamName = RemotingRefService.GetTeamName3rd(teamId3rd, TestingLanguage);

            // Assert.
            const string expectedTeamName = "Dukdang SamChay (KhaoNoi)";

            Assert.Equal(expectedTeamName, actualTeamName);
        }

        [Fact]
        public void GetLeagueName()
        {
            // Arrange.
            const string leagueId = "3";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName(leagueId, TestingLanguage);

            // Assert.
            const string expectedLeagueName = "*ENGLISH PREMIER LEAGUE";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Fact]
        public void GetLeagueName3rd()
        {
            // Arrange.
            const string leagueId3rd = "2";

            // Act.
            var actualLeagueName = RemotingRefService.GetLeagueName3rd(leagueId3rd, TestingLanguage);

            // Assert.
            const string expectedLeagueName = "KlongHae Songkhla";

            Assert.Equal(expectedLeagueName, actualLeagueName);
        }

        [Fact]
        public void GetCountryName()
        {
            // Arrange.
            const string countryCode = "KOR";

            // Act.
            var actualCountryName = RemotingRefService.GetCountryName(countryCode, TestingLanguage);

            // Assert.
            const string expectedCountryName = "Korea (Rep)";

            Assert.Equal(expectedCountryName, actualCountryName);
        }

        [Fact]
        public void GetOddsTypeName()
        {
            // Arrange.
            const string oddsTypeId = "2";

            // Act.
            var actualOddsTypeName = RemotingRefService.GetOddsTypeName(oddsTypeId, TestingLanguage);

            // Assert.
            const string expectedOddsTypeName = "HK";

            Assert.Equal(expectedOddsTypeName, actualOddsTypeName);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId()
        {
            // Arrange.
            const string refId = "AG_Baccarat";
            const string resourceId = "1";

            // Act.
            var actualResource = RemotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedResource = "Tie";

            Assert.Equal(expectedResource, actualResource);
        }

        [Fact]
        public void GetParentIdByBetTypeId()
        {
            // Arrange.
            const string betTypeId = "157";

            // Act.
            var actualParentBetTypeId = RemotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue);

            // Assert.
            const string expectedParentBetTypeId = "2";

            Assert.Equal(expectedParentBetTypeId, actualParentBetTypeId);
        }

        [Fact]
        public void GetCombinationTypeCombo()
        {
            // Arrange.
            const string betTeam = "120";
            const string betCheck = "4";
            const int handicap1 = 1;
            const int handicap2 = 3;

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
            const string codeId = "33231";

            // Act.
            var actualHorseTeamName = RemotingRefService.GetHorseTeamName(codeId, TestingLanguage);

            // Assert.
            const string expectedHorseTeamName = "SUN KING";

            Assert.Equal(expectedHorseTeamName, actualHorseTeamName);
        }

        [Fact]
        public void GetCasinoBetItem()
        {
            // Arrange.
            const string itemId = "1";

            // Act.
            var actualBetItem = RemotingRefService.GetCasinoBetItem(itemId, TestingLanguage);

            // Assert.
            const string expectedBetItem = "Banker";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        public void GetRouletteBetItem()
        {
            // Arrange.
            const string itemId = "2";

            // Act.
            var actualBetItem = RemotingRefService.GetRouletteBetItem(itemId, TestingLanguage);

            // Assert.
            const string expectedBetItem = "1";

            Assert.Equal(expectedBetItem, actualBetItem);
        }

        [Fact]
        public void GetKenoBetChoice()
        {
            // Arrange.
            const string kenoBetChoiceId = "1";

            // Act.
            var actualBetChoice = RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            const string expectedBetChoice = "Big";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        public void GetKenoBetTypeIdFromBetChoiceId()
        {
            // Arrange.
            const string kenoBetChoiceId = "1";

            // Act.
            RemotingRefService.GetKenoBetChoice(
                kenoBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            const string expectedBetTypeId = "1";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Fact]
        public void GetKenoBetType()
        {
            // Arrange.
            const string kenoBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetKenoBetType(kenoBetTypeId, TestingLanguage);

            // Assert.
            const string expectedBetTypeName = "Big/Small";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetFastMarketsChoice()
        {
            // Arrange.
            const string betTypeId = "222";
            const string betTeam = "2";
            const int handicap1 = 3;
            const int handicap2 = 4;

            // Act.
            var actualChoice = RemotingRefService.GetFastMarketsChoice(
                betTypeId,
                betTeam,
                handicap1,
                handicap2,
                TestingLanguage);

            // Assert.
            const string expectedChoice = "Corner - Yes (03:00-07:59)";

            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public void GetFastMarketsResult()
        {
            // Arrange.
            const string resultId = "3";

            // Act.
            var actualResult = RemotingRefService.GetFastMarketsResult(resultId, TestingLanguage);

            // Assert.
            const string expectedResult = "Goal, None";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetFastMarkets225Result()
        {
            // Arrange.
            const string betType255ResultId = "163";

            // Act.
            var actualResult = RemotingRefService.GetFastMarkets225Result(betType255ResultId, TestingLanguage);

            // Assert.
            const string expectedResult = "Penalty, Throw In, None";

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetLotteryBetChoice()
        {
            // Arrange.
            const string lotteryBetChoiceId = "40";

            // Act.
            var actualBetChoice = RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out _);

            // Assert.
            const string expectedBetChoice = "Green";

            Assert.Equal(expectedBetChoice, actualBetChoice);
        }

        [Fact]
        public void GetLotteryBetTypeIdFromBetChoiceId()
        {
            // Arrange.
            const string lotteryBetChoiceId = "40";

            // Act.
            RemotingRefService.GetLotteryBetChoice(
                lotteryBetChoiceId,
                TestingLanguage,
                out string actualBetTypeId);

            // Assert.
            const string expectedBetTypeId = "16";

            Assert.Equal(expectedBetTypeId, actualBetTypeId);
        }

        [Fact]
        public void GetLotteryBetType()
        {
            // Arrange.
            const string lotteryBetTypeId = "1";

            // Act.
            var actualBetTypeName = RemotingRefService.GetLotteryBetType(lotteryBetTypeId, TestingLanguage);

            // Assert.
            const string expectedBetTypeName = "1st Digit Big/Small";

            Assert.Equal(expectedBetTypeName, actualBetTypeName);
        }

        [Fact]
        public void GetBetRadarMatchCodeFormat()
        {
            // Arrange.
            const string matchCode = "101090 {10}";

            // Act.
            var actualMatch = RemotingRefService.GetBetRadarMatchCodeFormat(matchCode, TestingLanguage);

            // Assert.
            const string expectedMatch = "101090 Match Day 3 Groups C&D";

            Assert.Equal(expectedMatch, actualMatch);
        }

        [Fact]
        public void GetDisplayedTypeId()
        {
            // Arrange.
            const string betTypeId = "157";

            // Act.
            var actualDisplayedBetTypeId = RemotingRefService.GetDisplayedTypeId(betTypeId);

            // Assert.
            const string expectedBetTypeId = "2";

            Assert.Equal(expectedBetTypeId, actualDisplayedBetTypeId);
        }

        [Fact]
        public void GetDisplayedBetTypeName()
        {
            // Arrange.
            const string betTypeId = "407";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(betTypeId, TestingLanguage);

            // Assert.
            const string expectedDisplayedBetTypeName = "Exact Home Team Goals";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void GetDisplayedViaLuckyBetTypeName()
        {
            // Arrange.
            const string betTypeId = "407";
            const long refNo = 100_000_000_001;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                refNo,
                TestingLanguage);

            // Assert.
            const string expectedDisplayedBetTypeName = "Exact Home Team Goals (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("Get bet type name stored in DB, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetDisplayedBetTypeNameContaingPlaceholders()
        {
            // Arrange.
            const string betTypeId = "1316";
            const string resourceId = "01";

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedDisplayedBetTypeName = "Set 1 Game Handicap";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        [Description("Get Via Lucky bet type name, then replace X, Y, or Z in its name with values in resourceId")]
        public void GetDisplayedViaLuckyBetTypeNameContainingPlaceholders()
        {
            // Arrange.
            const string betTypeId = "1316";
            const string resourceId = "02";
            const long refNo = 100_000_000_001;

            // Act.
            var actualDisplayedBetTypeName = RemotingRefService.GetDisplayedBetTypeName(
                betTypeId,
                resourceId,
                refNo,
                TestingLanguage,
                DefaultValue);

            // Assert.
            const string expectedDisplayedBetTypeName = "Set 2 Game Handicap (Via Lucky)";

            Assert.Equal(expectedDisplayedBetTypeName, actualDisplayedBetTypeName);
        }

        [Fact]
        public void GetBetBuilderSelectionNames()
        {
            // Arrange.
            var selectionIds = new[] { 68, 69 };

            // Act.
            var actualBetBuilderSelectionNames = RemotingRefService.GetBetBuilderSelectionNames(selectionIds, TestingLanguage);

            // Assert.
            var expectedBetBuilderSelectionNames = new[]
            {
                "Correct score - St Etienne 2-0",
                "Player goals - Romain Hamouma score anytime"
            };

            Assert.Equal(expectedBetBuilderSelectionNames, actualBetBuilderSelectionNames);
        }
    }
}