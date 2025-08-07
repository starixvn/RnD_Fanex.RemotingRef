using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.RemotingRef.NetCore.Utilities.RefData;
using NSubstitute;
using PublicEnum.Language;
using RefClient.Factory;
using RefClient.Service;
using RefClient.Service.IService;
using RefDataModelStandard.DataModels;
using RefDataModelStandard.Enums;
using RefDataModelStandard.Models.Request;
using Xunit;

namespace Fanex.RemotingRef.Service.Tests.Utilities.RefData
{
    public class RefDataHelperTests
    {
        private readonly Fixture _fixture;
        private readonly IBetRefClientService _mockBetRefClient;
        private readonly IRefDataClient _mockRefDataClient;
        private readonly IThirdPartyRefClientService _mockThirdPartyRefClientService;
        private readonly RefDataHelper _refDataHelper;

        public RefDataHelperTests()
        {
            _fixture = new Fixture();

            // Create mocks
            _mockBetRefClient = Substitute.For<IBetRefClientService>();
            _mockRefDataClient = Substitute.For<IRefDataClient>();
            _mockThirdPartyRefClientService = Substitute.For<IThirdPartyRefClientService>();

            _refDataHelper = new RefDataHelper(_mockBetRefClient, _mockRefDataClient, _mockThirdPartyRefClientService);
        }

        [Fact]
        public void GetBetTypeName_WithValidBetTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var betTypeId = "123";
            var resourceId = "resource1";
            var language = "en";
            var expectedResult = "Test Bet Type";

            _mockBetRefClient.GetBetTypeName(Arg.Any<GetBetTypeNameModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetBetTypeName(betTypeId, resourceId, null, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetBetTypeName(Arg.Any<GetBetTypeNameModel>());
        }

        [Fact]
        public void GetBetTypeName_WithInvalidBetTypeId_ReturnsNull()
        {
            // Arrange
            var betTypeId = "invalid";
            var resourceId = "resource1";
            var language = "en";

            // Act
            var result = _refDataHelper.GetBetTypeName(betTypeId, resourceId, null, language);

            // Assert
            Assert.Null(result);
            _mockBetRefClient.DidNotReceive().GetBetTypeName(Arg.Any<GetBetTypeNameModel>());
        }

        [Fact]
        public void GetBetTypeName_WithLuckyViaRefNo_ReturnsCombinedResult()
        {
            // Arrange
            var betTypeId = "123";
            var resourceId = "resource1";
            var refNo = 100_000_000_001L;
            var language = "en";
            var betTypeName = "Test Bet Type";
            var luckyVia = "Lucky Via";
            var expectedResult = $"{betTypeName} {luckyVia}";

            _mockBetRefClient.GetBetTypeName(Arg.Any<GetBetTypeNameModel>()).Returns(betTypeName);
            _mockBetRefClient.GetComboParlayLuckyBet(Arg.Any<GetComboParlayLuckyBetModel>()).Returns(luckyVia);

            // Act
            var result = _refDataHelper.GetBetTypeName(betTypeId, resourceId, refNo, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetBetTypeName(Arg.Any<GetBetTypeNameModel>());
            _mockBetRefClient.Received(1).GetComboParlayLuckyBet(Arg.Any<GetComboParlayLuckyBetModel>());
        }

        [Theory]
        [InlineData("8810")]
        [InlineData("8811")]
        [InlineData("8812")]
        [InlineData("8813")]
        [InlineData("8823")]
        [InlineData("8824")]
        public void GetBetTypeName_WithSpecialBaseballBetType_ReturnsBaseballSpecificResult(string betTypeId)
        {
            // Arrange
            var resourceId = "1";
            var language = "en-US";
            var expectedResult = "Baseball Bet Type First ";

            _mockBetRefClient.GetBetTypeName(Arg.Any<GetBetTypeNameModel>()).Returns("Baseball Bet Type X ");

            // Act
            var result = _refDataHelper.GetBetTypeName(betTypeId, resourceId, null, language);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSportName_WithValidSportTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var sportTypeId = "123";
            var language = "en";
            var expectedResult = "Football";

            _mockRefDataClient.GetSportName(Arg.Any<GetSportNameModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetSportName(sportTypeId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetSportName(Arg.Any<GetSportNameModel>());
        }

        [Fact]
        public void GetSportName_WithInvalidSportTypeId_ReturnsNull()
        {
            // Arrange
            var sportTypeId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetSportName(sportTypeId, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.DidNotReceive().GetSportName(Arg.Any<GetSportNameModel>());
        }

        [Fact]
        public void GetTeamName_WithValidTeamId_ReturnsExpectedResult()
        {
            // Arrange
            var teamId = "123";
            var language = "en";
            var expectedResult = "Team Name";

            _mockRefDataClient.GetTeamName(Arg.Any<GetTeamNameModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetTeamName(teamId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetTeamName(Arg.Any<GetTeamNameModel>());
        }

        [Fact]
        public void GetTeamName_WithInvalidTeamId_ReturnsNull()
        {
            // Arrange
            var teamId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetTeamName(teamId, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.DidNotReceive().GetTeamName(Arg.Any<GetTeamNameModel>());
        }

        [Fact]
        public void GetTeamName3rd_WithValidTeam3rdId_ReturnsExpectedResult()
        {
            // Arrange
            var team3rdId = "123";
            var language = "en";
            var expectedResult = "Third Party Team Name";

            _mockRefDataClient.GetThirdPartyTeamName(Arg.Any<GetThirdPartyTeamNameModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetTeamName3rd(team3rdId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetThirdPartyTeamName(Arg.Any<GetThirdPartyTeamNameModel>());
        }

        [Fact]
        public void GetTeamName3rd_WithInvalidTeam3rdId_ReturnsNull()
        {
            // Arrange
            var team3rdId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetTeamName3rd(team3rdId, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.DidNotReceive().GetThirdPartyTeamName(Arg.Any<GetThirdPartyTeamNameModel>());
        }

        [Fact]
        public void GetLeagueName_WithValidLeagueId_ReturnsExpectedResult()
        {
            // Arrange
            var leagueId = "123";
            var language = "en";
            var expectedResult = "League Name";

            _mockRefDataClient.GetLeagueName(Arg.Any<GetLeagueNameModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetLeagueName(leagueId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetLeagueName(Arg.Any<GetLeagueNameModel>());
        }

        [Fact]
        public void GetLeagueName_WithInvalidLeagueId_ReturnsNull()
        {
            // Arrange
            var leagueId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetLeagueName(leagueId, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.DidNotReceive().GetLeagueName(Arg.Any<GetLeagueNameModel>());
        }

        [Fact]
        public void GetLeagueName3rd_WithValidLeague3rdId_ReturnsExpectedResult()
        {
            // Arrange
            var league3rdId = "123";
            var language = "en";
            var expectedResult = "Third Party League Name";

            _mockThirdPartyRefClientService.GetThirdPartyLeagueName(Arg.Any<GetThirdPartyLeagueNameModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetLeagueName3rd(league3rdId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockThirdPartyRefClientService.Received(1).GetThirdPartyLeagueName(Arg.Any<GetThirdPartyLeagueNameModel>());
        }

        [Fact]
        public void GetLeagueName3rd_WithInvalidLeague3rdId_ReturnsNull()
        {
            // Arrange
            var league3rdId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetLeagueName3rd(league3rdId, language);

            // Assert
            Assert.Null(result);
            _mockThirdPartyRefClientService.DidNotReceive().GetThirdPartyLeagueName(Arg.Any<GetThirdPartyLeagueNameModel>());
        }

        [Fact]
        public void GetCountryName_WithValidCountryCode_ReturnsExpectedResult()
        {
            // Arrange
            var countryCode = "US";
            var language = "en";
            var expectedResult = "United States";

            _mockRefDataClient.GetCountryName(Arg.Any<GetCountryNameModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetCountryName(countryCode, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetCountryName(Arg.Any<GetCountryNameModel>());
        }

        [Fact]
        public void GetOddsTypeName_WithValidOddsTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var oddsTypeId = "123";
            var language = "en";
            var expectedResult = "Decimal Odds";

            _mockBetRefClient.GetOddsTypeName(Arg.Any<GetOddsTypeNameModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetOddsTypeName(oddsTypeId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetOddsTypeName(Arg.Any<GetOddsTypeNameModel>());
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId_WithValidData_ReturnsExpectedResult()
        {
            // Arrange
            var refId = "ref123";
            var resourceId = "resource1";
            var language = "en";
            var expectedResult = "Resource Name";

            var resourceData = new Dictionary<string, ResourceDataModel>
            {
                { resourceId, new ResourceDataModel { RefId = refId, ResourceId = resourceId, ResourceEn = expectedResult } }
            };

            _mockRefDataClient.GetResourceDictionary(Arg.Any<GetResourceDictionaryModel>()).Returns(Task.FromResult(resourceData));

            // Act
            var result = _refDataHelper.GetResourceDataByRefIdAndResourceId(refId, resourceId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            //_mockRefDataClient.Received(1).GetResourceDictionary(Arg.Any<GetResourceDictionaryModel>());
        }

        //[Fact]
        //public void GetResourceDataByRefIdAndResourceId_WithInvalidResourceId_ReturnsEmptyString()
        //{
        //    // Arrange
        //    var refId = "ref123";
        //    var resourceId = "invalid";
        //    var language = "en";

        //    var resourceData = new Dictionary<string, object>
        //    {
        //        { "other", new { Name = new Dictionary<LanguageEnum, string> { { LanguageEnum.En, "Other" } } } }
        //    };

        //    _mockRefDataClient.GetResourceDictionary(Arg.Any<GetResourceDictionaryModel>()).Returns(Task.FromResult(resourceData));

        //    // Act
        //    var result = _refDataHelper.GetResourceDataByRefIdAndResourceId(refId, resourceId, language);

        //    // Assert
        //    Assert.Equal(string.Empty, result);
        //}

        [Fact]
        public void GetParentIdByBetTypeId_WithValidBetTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var betTypeId = "123";
            var expectedResult = "456";

            _mockBetRefClient.GetParentIdByBetTypeId(Arg.Any<GetParentIdByBetTypeIdModel>()).Returns(int.Parse(expectedResult));

            // Act
            var result = _refDataHelper.GetParentIdByBetTypeId(betTypeId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetParentIdByBetTypeId(Arg.Any<GetParentIdByBetTypeIdModel>());
        }

        [Fact]
        public void GetParentIdByBetTypeId_WithInvalidBetTypeId_ReturnsNull()
        {
            // Arrange
            var betTypeId = "invalid";

            // Act
            var result = _refDataHelper.GetParentIdByBetTypeId(betTypeId);

            // Assert
            Assert.Null(result);
            _mockBetRefClient.DidNotReceive().GetParentIdByBetTypeId(Arg.Any<GetParentIdByBetTypeIdModel>());
        }

        [Fact]
        public void GetCombinationTypeCombo_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var betTeam = "team1";
            var betCheck = "2";
            var handicap1 = 1;
            var handicap2 = 2;
            var language = "en";
            var expectedResult = new List<string> { "Combo1", "Combo2" };

            _mockBetRefClient.GetCombinationTypeCombo(Arg.Any<GetCombinationTypeComboModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetCombinationTypeCombo(Arg.Any<GetCombinationTypeComboModel>());
        }

        [Fact]
        public void GetCombinationTypeCombo_WithInvalidBetCheck_ReturnsEmptyList()
        {
            // Arrange
            var betTeam = "team1";
            var betCheck = "Invalid";
            var handicap1 = 1;
            var handicap2 = 2;
            var language = "en";

            // Act
            var result = _refDataHelper.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, language);

            // Assert
            Assert.Empty(result);
            _mockBetRefClient.DidNotReceive().GetCombinationTypeCombo(Arg.Any<GetCombinationTypeComboModel>());
        }

        [Fact]
        public void GetHorseTeamName_AlwaysReturnsCodeId()
        {
            // Arrange
            var codeId = "HORSE123";
            var language = "en";

            // Act
            var result = _refDataHelper.GetHorseTeamName(codeId, language);

            // Assert
            Assert.Equal(codeId, result);
        }

        [Fact]
        public void GetCasinoBetItem_WithValidItemId_ReturnsExpectedResult()
        {
            // Arrange
            var itemId = "item123";
            var language = "en";
            var expectedResult = "Casino Item";

            _mockBetRefClient.GetCasinoBetItem(Arg.Any<GetCasinoBetItemModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetCasinoBetItem(itemId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetCasinoBetItem(Arg.Any<GetCasinoBetItemModel>());
        }

        [Fact]
        public void GetRouletteBetItem_WithValidItemId_ReturnsExpectedResult()
        {
            // Arrange
            var itemId = "item123";
            var language = "en";
            var expectedResult = "Roulette Item";

            _mockBetRefClient.GetRouletteBetItem(Arg.Any<GetRouletteBetItemModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetRouletteBetItem(itemId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetRouletteBetItem(Arg.Any<GetRouletteBetItemModel>());
        }

        //[Fact]
        //public void GetKenoBetChoice_WithValidChoiceId_ReturnsExpectedResult()
        //{
        //    // Arrange
        //    var kenoBetChoiceId = "choice123";
        //    var language = "en";
        //    var expectedName = "Keno Choice";
        //    var expectedId = "456";

        //    var mockModel = new { Name = new Dictionary<LanguageEnum, string> { { LanguageEnum.En, expectedName } }, Sort1 = int.Parse(expectedId) };
        //    _mockBetRefClient.GetKenoBetChoice(Arg.Any<GetKenoBetChoiceModel>()).Returns(mockModel);

        //    // Act
        //    var result = _refDataHelper.GetKenoBetChoice(kenoBetChoiceId, language);

        //    // Assert
        //    Assert.Equal(expectedName, result.betChoiceName);
        //    Assert.Equal(expectedId, result.bettypeId);
        //    _mockBetRefClient.Received(1).GetKenoBetChoice(Arg.Any<GetKenoBetChoiceModel>());
        //}

        [Fact]
        public void GetKenoBetType_WithValidTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var kenoBetTypeId = "type123";
            var language = "en";
            var expectedResult = "Keno Bet Type";

            _mockBetRefClient.GetKenoBetType(Arg.Any<GetKenoBetTypeModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetKenoBetType(kenoBetTypeId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetKenoBetType(Arg.Any<GetKenoBetTypeModel>());
        }

        [Fact]
        public void GetFastMarketsChoice_WithValidParameters_ReturnsExpectedResult()
        {
            // Arrange
            var betTypeId = "123";
            var betTeam = "456";
            var handicap1 = 1;
            var handicap2 = 2;
            var language = "en";
            var expectedResult = "Fast Market Choice";

            _mockRefDataClient.GetFastMarketChoice(Arg.Any<GetFastMarketsChoiceModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetFastMarketChoice(Arg.Any<GetFastMarketsChoiceModel>());
        }

        [Fact]
        public void GetFastMarketsChoice_WithInvalidParameters_ReturnsNull()
        {
            // Arrange
            var betTypeId = "invalid";
            var betTeam = "456";
            var handicap1 = 1;
            var handicap2 = 2;
            var language = "en";

            // Act
            var result = _refDataHelper.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.DidNotReceive().GetFastMarketChoice(Arg.Any<GetFastMarketsChoiceModel>());
        }

        [Fact]
        public void GetFastMarketsResult_WithValidResultId_ReturnsExpectedResult()
        {
            // Arrange
            var resultId = "1";
            var language = "en";
            var expectedResult = "Fast Market Result";
            var resultList = new List<string> { expectedResult };

            _mockRefDataClient.GetFastMarketBetOptionName(Arg.Any<GetFastMarketsResultModel>()).Returns(Task.FromResult(resultList));

            // Act
            var result = _refDataHelper.GetFastMarketsResult(resultId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetFastMarketBetOptionName(Arg.Any<GetFastMarketsResultModel>());
        }

        [Fact]
        public void GetFastMarketsResult_WithInvalidResultId_ReturnsResultId()
        {
            // Arrange
            var resultId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetFastMarketsResult(resultId, language);

            // Assert
            Assert.Equal(resultId, result);
            _mockRefDataClient.DidNotReceive().GetFastMarketBetOptionName(Arg.Any<GetFastMarketsResultModel>());
        }

        [Fact]
        public void GetFastMarkets225Result_WithValidResultId_ReturnsExpectedResult()
        {
            // Arrange
            var resultId = "1";
            var language = "en";
            var expectedResult = "Fast Market 225 Result";
            var resultList = new List<string> { expectedResult };

            _mockRefDataClient.GetFastMarketBetOptionNameOfBetType225(Arg.Any<GetFastMarketsResultModel>()).Returns(Task.FromResult(resultList));

            // Act
            var result = _refDataHelper.GetFastMarkets225Result(resultId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetFastMarketBetOptionNameOfBetType225(Arg.Any<GetFastMarketsResultModel>());
        }

        [Fact]
        public void GetFastMarkets225Result_WithInvalidResultId_ReturnsResultId()
        {
            // Arrange
            var resultId = "invalid";
            var language = "en";

            // Act
            var result = _refDataHelper.GetFastMarkets225Result(resultId, language);

            // Assert
            Assert.Equal(resultId, result);
            _mockRefDataClient.DidNotReceive().GetFastMarketBetOptionNameOfBetType225(Arg.Any<GetFastMarketsResultModel>());
        }

        //[Fact]
        //public void GetLotteryBetChoice_WithValidChoiceId_ReturnsExpectedResult()
        //{
        //    // Arrange
        //    var lotteryBetChoiceId = "choice123";
        //    var language = "en";
        //    var expectedName = "Lottery Choice";
        //    var expectedId = "789";

        //    var mockModel = new { Name = new Dictionary<LanguageEnum, string> { { LanguageEnum.En, expectedName } }, Sort1 = int.Parse(expectedId) };
        //    _mockRefDataClient.GetLotteryBetChoice(Arg.Any<GetLotteryBetChoiceModel>()).Returns(Task.FromResult(mockModel));

        //    // Act
        //    var result = _refDataHelper.GetLotteryBetChoice(lotteryBetChoiceId, language);

        //    // Assert
        //    Assert.Equal(expectedName, result.name);
        //    Assert.Equal(expectedId, result.id);
        //    _mockRefDataClient.Received(1).GetLotteryBetChoice(Arg.Any<GetLotteryBetChoiceModel>());
        //}

        [Fact]
        public void GetLotteryBetType_WithValidTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var lotteryBetTypeId = "type123";
            var language = "en";
            var expectedResult = "Lottery Bet Type";

            _mockBetRefClient.GetLotteryBetType(Arg.Any<GetLotteryBetTypeModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetLotteryBetType(lotteryBetTypeId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetLotteryBetType(Arg.Any<GetLotteryBetTypeModel>());
        }

        [Fact]
        public void GetBetRadarMatchCodeFormat_WithValidMatchCode_ReturnsExpectedResult()
        {
            // Arrange
            var matchCode = "MATCH123";
            var language = "en";
            var expectedResult = "Formatted Match Code";

            _mockRefDataClient.GetBetradarMatchCodeFormat(Arg.Any<GetBetradarMatchCodeFormatModel>()).Returns(Task.FromResult(expectedResult));

            // Act
            var result = _refDataHelper.GetBetRadarMatchCodeFormat(matchCode, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetBetradarMatchCodeFormat(Arg.Any<GetBetradarMatchCodeFormatModel>());
        }

        [Fact]
        public void GetDisplayedTypeId_WithValidBetTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var betTypeId = "123";
            var expectedResult = "456";

            _mockBetRefClient.GetBetTypeOfDisplayTypeId(Arg.Any<GetBetTypeOfDisplayTypeIdModel>()).Returns(int.Parse(expectedResult));

            // Act
            var result = _refDataHelper.GetDisplayedTypeId(betTypeId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetBetTypeOfDisplayTypeId(Arg.Any<GetBetTypeOfDisplayTypeIdModel>());
        }

        [Fact]
        public void GetDisplayedTypeId_WithInvalidBetTypeId_ReturnsOriginalBetTypeId()
        {
            // Arrange
            var betTypeId = "invalid";

            // Act
            var result = _refDataHelper.GetDisplayedTypeId(betTypeId);

            // Assert
            Assert.Equal(betTypeId, result);
            _mockBetRefClient.DidNotReceive().GetBetTypeOfDisplayTypeId(Arg.Any<GetBetTypeOfDisplayTypeIdModel>());
        }

        [Fact]
        public void GetDisplayedTypeId_WhenDisplayIdIsZero_ReturnsOriginalBetTypeId()
        {
            // Arrange
            var betTypeId = "123";

            _mockBetRefClient.GetBetTypeOfDisplayTypeId(Arg.Any<GetBetTypeOfDisplayTypeIdModel>()).Returns(0);

            // Act
            var result = _refDataHelper.GetDisplayedTypeId(betTypeId);

            // Assert
            Assert.Equal(betTypeId, result);
            _mockBetRefClient.Received(1).GetBetTypeOfDisplayTypeId(Arg.Any<GetBetTypeOfDisplayTypeIdModel>());
        }

        [Fact]
        public void GetBetBuilderSelectionName_WithValidSelectionId_ReturnsExpectedResult()
        {
            // Arrange
            var selectionId = 123;
            var language = "en";
            var expectedResult = "Selection Name";
            var selectionNames = new Dictionary<int, string> { { selectionId, expectedResult } };

            _mockRefDataClient.GetBetBuilderSelectionName(Arg.Any<GetBetBuilderSelectionNameModel>()).Returns(Task.FromResult(selectionNames));

            // Act
            var result = _refDataHelper.GetBetBuilderSelectionName(selectionId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRefDataClient.Received(1).GetBetBuilderSelectionName(Arg.Any<GetBetBuilderSelectionNameModel>());
        }

        [Fact]
        public void GetBetBuilderSelectionName_WithInvalidSelectionId_ReturnsNull()
        {
            // Arrange
            var selectionId = 123;
            var language = "en";
            var selectionNames = new Dictionary<int, string> { { 456, "Other Selection" } };

            _mockRefDataClient.GetBetBuilderSelectionName(Arg.Any<GetBetBuilderSelectionNameModel>()).Returns(Task.FromResult(selectionNames));

            // Act
            var result = _refDataHelper.GetBetBuilderSelectionName(selectionId, language);

            // Assert
            Assert.Null(result);
            _mockRefDataClient.Received(1).GetBetBuilderSelectionName(Arg.Any<GetBetBuilderSelectionNameModel>());
        }

        //[Fact]
        //public void GetTableGamesBetChoice_WithValidChoiceId_ReturnsExpectedResult()
        //{
        //    // Arrange
        //    var tableGamesBetChoiceId = "choice123";
        //    var language = "en";
        //    var expectedName = "Table Game Choice";
        //    var expectedId = "789";

        //    var mockModel = new { Name = new Dictionary<LanguageEnum, string> { { LanguageEnum.En, expectedName } }, Sort1 = int.Parse(expectedId) };
        //    _mockBetRefClient.GetTableGameBetChoice(Arg.Any<GetTableGameBetChoiceModel>()).Returns(mockModel);

        //    // Act
        //    var result = _refDataHelper.GetTableGamesBetChoice(tableGamesBetChoiceId, language);

        //    // Assert
        //    Assert.Equal(expectedName, result.name);
        //    Assert.Equal(expectedId, result.id);
        //    _mockBetRefClient.Received(1).GetTableGameBetChoice(Arg.Any<GetTableGameBetChoiceModel>());
        //}

        [Fact]
        public void GetTableGamesBetChoice_WithNullModel_ReturnsEmptyStrings()
        {
            // Arrange
            var tableGamesBetChoiceId = "choice123";
            var language = "en";

            _mockBetRefClient.GetTableGameBetChoice(Arg.Any<GetTableGameBetChoiceModel>()).Returns((object)null);

            // Act
            var result = _refDataHelper.GetTableGamesBetChoice(tableGamesBetChoiceId, language);

            // Assert
            Assert.Equal(string.Empty, result.name);
            Assert.Equal(string.Empty, result.id);
            _mockBetRefClient.Received(1).GetTableGameBetChoice(Arg.Any<GetTableGameBetChoiceModel>());
        }

        [Fact]
        public void GetTableGamesBetType_WithValidTypeId_ReturnsExpectedResult()
        {
            // Arrange
            var tableGamesBetTypeId = "type123";
            var language = "en";
            var expectedResult = "Table Game Bet Type";

            _mockBetRefClient.GetTableGameBetType(Arg.Any<GetTableGameBetTypeModel>()).Returns(expectedResult);

            // Act
            var result = _refDataHelper.GetTableGamesBetType(tableGamesBetTypeId, language);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockBetRefClient.Received(1).GetTableGameBetType(Arg.Any<GetTableGameBetTypeModel>());
        }
    }
}