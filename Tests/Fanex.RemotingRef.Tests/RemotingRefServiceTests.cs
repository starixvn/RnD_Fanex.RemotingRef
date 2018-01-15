namespace Fanex.RemotingRef.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Fanex.RemotingRef.Utilities.WebClient;
    using NSubstitute;
    using Xunit;

    public class RemotingRefServiceTests
    {
        private readonly IHttpHandler _client;
        private readonly ISerializer _serializer;
        private readonly IRemotingRefService _remotingRefService;

        public RemotingRefServiceTests()
        {
            _client = Substitute.For<IHttpHandler>();
            _serializer = Substitute.For<ISerializer>();
            RefDataTypeContainer.ConnectTo(new Uri("http://localhost:10000"));
            RefDataTypeHandlerOptions.SupportedLanguageCodes = new[] { RefDataTypeHandlerOptions.DefaultLanguage };
            var httpResponseMessage = Substitute.For<HttpResponseMessage>();
            httpResponseMessage.Content = new StreamContent(new MemoryStream(new byte[] { }));
            _client.GetAsync(Arg.Any<string>()).Returns(httpResponseMessage);
            _serializer.Deserialize<Dictionary<string, string>>(Arg.Any<Stream>()).Returns(new Dictionary<string, string>());
            _remotingRefService = new RemotingRefService(_client, _serializer);
        }

        [Fact]
        public void GetBetTypeName_Always_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetBetTypeName("1", "en-US");

            // Assert
            _client.Received().GetAsync(Arg.Is<string>(url => url == "/api/ref/BetTypeName?betTypeId=1&languages=en-US&"));
        }

        [Fact]
        public void GetBetTypeName_WithRefNo_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetBetTypeName("1", "en-US", 100000000000);

            // Assert
            _client.Received().GetAsync(Arg.Is<string>(url => url == "/api/ref/BetTypeNameWithRefNo?betTypeId=1&refNo=100000000000&languages=en-US&"));
        }

        [Fact]
        public void GetBetTypeName_WithRefNoAndDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var betTypeName = _remotingRefService.GetBetTypeName("1", "en-US", 100000000000, "betType");

            // Assert
            _client.Received().GetAsync(Arg.Is<string>(url => url == "/api/ref/BetTypeNameWithRefNo?betTypeId=1&refNo=100000000000&languages=en-US&"));
            Assert.Equal("betType", betTypeName);
        }

        [Fact]
        public void GetBetTypeName_WithResourceId_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetBetTypeName("1", "en-US", string.Empty, "2");

            // Assert
            _client.Received().GetAsync(Arg.Is<string>(url => url == "/api/ref/BetTypeNameWithResourceId?betTypeId=1&resourceId=2&languages=en-US&"));
        }

        [Fact]
        public void GetBetTypeName_WithRefNoAndResourceId_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetBetTypeName("1", "en-US", string.Empty, "2", 100000000000);

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/BetTypeNameWithRefNoAndResourceId?betTypeId=1&resourceId=2&refNo=100000000000&languages=en-US&"));
        }

        [Fact]
        public void GetSportName_WithoutDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetSportName("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/sportName?sportTypeId=1&languages=en-US&"));
        }

        [Fact]
        public void GetSportName_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var sportName = _remotingRefService.GetSportName("1", "en-US", "sportName");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/sportName?sportTypeId=1&languages=en-US&"));
            Assert.Equal("sportName", sportName);
        }

        [Fact]
        public void GetTeamName_WithoutDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetTeamName("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/teamName?teamId=1&languages=en-US&"));
        }

        [Fact]
        public void GetTeamName_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var teamName = _remotingRefService.GetTeamName("1", "en-US", "teamName");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/teamName?teamId=1&languages=en-US&"));
            Assert.Equal("teamName", teamName);
        }

        [Fact]
        public void GetTeamName3rd_WithoutDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetTeamName3rd("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/teamName3rd?team3rdId=1&languages=en-US&"));
        }

        [Fact]
        public void GetTeamName3rd_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var teamName3rd = _remotingRefService.GetTeamName3rd("1", "en-US", "teamName3rd");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/teamName3rd?team3rdId=1&languages=en-US&"));
            Assert.Equal("teamName3rd", teamName3rd);
        }

        [Fact]
        public void GetLeagueName_WithoutDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetLeagueName("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/leagueName?leagueId=1&languages=en-US&"));
        }

        [Fact]
        public void GetLeagueName_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var leagueName = _remotingRefService.GetLeagueName("1", "en-US", "leagueName");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/leagueName?leagueId=1&languages=en-US&"));
            Assert.Equal("leagueName", leagueName);
        }

        [Fact]
        public void GetLeagueName3rd_WithoutDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            _remotingRefService.GetLeagueName3rd("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/leagueName3rd?league3rdId=1&languages=en-US&"));
        }

        [Fact]
        public void GetLeagueName3rd_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var leagueName3rd = _remotingRefService.GetLeagueName3rd("1", "en-US", "leagueName3rd");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/leagueName3rd?league3rdId=1&languages=en-US&"));
            Assert.Equal("leagueName3rd", leagueName3rd);
        }

        [Fact]
        public void GetCountryName_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var countryName = _remotingRefService.GetCountryName("1", "en-US", "countryName");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/countryName?countryCode=1&languages=en-US&"));
            Assert.Equal("countryName", countryName);
        }

        [Fact]
        public void GetOddsTypeName_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var oddsTypeName = _remotingRefService.GetOddsTypeName("1", "en-US", "oddsTypeName");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/oddsTypeName?oddsTypeId=1&languages=en-US&"));
            Assert.Equal("oddsTypeName", oddsTypeName);
        }

        [Fact]
        public void GetResourceDataByRefIdAndResourceId_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var resourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId("1", "2", "en-US", "resourceData");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/resourceData?refId=1&resourceId=2&languages=en-US&"));
            Assert.Equal("resourceData", resourceData);
        }

        [Fact]
        public void GetParentIdByBetTypeId_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var betType = _remotingRefService.GetParentIdByBetTypeId("1", "betType");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/parentIdByBetTypeId?betTypeId=1&languages=en-US&"));
            Assert.Equal("betType", betType);
        }

        [Fact]
        public void GetCombinationTypeCombo_Always_GetBetTypeNameWithCorrectParamters()
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, string[]>>(Arg.Any<Stream>()).Returns(new Dictionary<string, string[]>());

            // Act
            _remotingRefService.GetCombinationTypeCombo("1", "2", 3, 4, "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/combinationTypeCombo?betTeam=1&betCheck=2&handicap1=3&handicap2=4&languages=en-US&"));
        }

        [Fact]
        public void GetHorseTeamName_Always_ThrowNotSupportedException()
        {
            // Act
            var testDelegate = new Action(() => _remotingRefService.GetHorseTeamName("1", "en-US", string.Empty));

            // Assert
            Assert.Throws<NotSupportedException>(testDelegate);
        }

        [Fact]
        public void GetCasinoBetItem_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var casinoBetItem = _remotingRefService.GetCasinoBetItem("1", "en-US", "casinoBetItem");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/casinoBetItem?itemId=1&languages=en-US&"));
            Assert.Equal("casinoBetItem", casinoBetItem);
        }

        [Fact]
        public void GetRouletteBetItem_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Act
            var rouletteBetItem = _remotingRefService.GetRouletteBetItem("1", "en-US", "rouletteBetItem");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/rouletteBetItem?itemId=1&languages=en-US&"));
            Assert.Equal("rouletteBetItem", rouletteBetItem);
        }

        [Fact]
        public void GetKenoBetChoice_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("1", "2") } });

            // Act
            var kenoBetChoice = _remotingRefService.GetKenoBetChoice("1", "en-US", "kenoBetChoice", out string kenoBetTypeId);

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/kenoBetChoice?kenoBetChoiceId=1&languages=en-US&"));
            Assert.Equal("1", kenoBetChoice);
            Assert.Equal("2", kenoBetTypeId);
        }

        [Theory]
        [InlineData("betType", "betType")]
        [InlineData("", "kenoBetType")]
        [InlineData(null, "kenoBetType")]
        public void GetKenoBetType_WithDefaultValue_GetBetTypeNameWithCorrectParamters(string returnValue, string expectedValue)
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
             .Returns(new Dictionary<string, string> { { "en-US", returnValue } });

            // Act
            var kenoBetType = _remotingRefService.GetKenoBetType("1", "en-US", "kenoBetType");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/kenoBetType?kenoBetTypeId=1&languages=en-US&"));
            Assert.Equal(expectedValue, kenoBetType);
        }

        [Fact]
        public void GetFastMarketsChoice_Always_GetBetTypeNameWithCorrectParamters()
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
             .Returns(new Dictionary<string, string> { { "en-US", "fastMarketsChoice" } });

            // Act
            var fastMarketsChoice = _remotingRefService.GetFastMarketsChoice("1", "2", 3, 4, "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/fastMarketsChoice?betTypeId=1&betTeam=2&handicap1=3&handicap2=4&languages=en-US&"));
            Assert.Equal("fastMarketsChoice", fastMarketsChoice);
        }

        [Fact]
        public void GetFastMarkets225Result_Always_GetBetTypeNameWithCorrectParamters()
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
             .Returns(new Dictionary<string, string> { { "en-US", "fastMarkets225Result" } });

            // Act
            var fastMarkets225Result = _remotingRefService.GetFastMarkets225Result("1", "en-US");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/fastMarkets225Result?resultId=1&languages=en-US&"));
            Assert.Equal("fastMarkets225Result", fastMarkets225Result);
        }

        [Fact]
        public void GetLotteryBetChoice_WithDefaultValue_GetBetTypeNameWithCorrectParamters()
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, Tuple<string, string>>>(Arg.Any<Stream>())
                .Returns(new Dictionary<string, Tuple<string, string>> { { "en-US", new Tuple<string, string>("1", "2") } });

            // Act
            var lotteryBetChoice = _remotingRefService.GetLotteryBetChoice("1", "en-US", "lotteryBetChoice", out string lotteryBetType);

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/lotteryBetChoice?betChoiceId=1&languages=en-US&"));
            Assert.Equal("1", lotteryBetChoice);
            Assert.Equal("2", lotteryBetType);
        }

        [Theory]
        [InlineData("betType", "betType")]
        [InlineData("", "lotteryBetType")]
        [InlineData(null, "lotteryBetType")]
        public void GetLotteryBetType_WithDefaultValue_GetBetTypeNameWithCorrectParamters(string returnValue, string expectedValue)
        {
            // Arrange
            _serializer.Deserialize<Dictionary<string, string>>(Arg.Any<Stream>())
             .Returns(new Dictionary<string, string> { { "en-US", returnValue } });

            // Act
            var lotteryBetType = _remotingRefService.GetLotteryBetType("1", "en-US", "lotteryBetType");

            // Assert
            _client.Received().GetAsync(
                Arg.Is<string>(url => url == "/api/ref/lotteryBetType?betTypeId=1&languages=en-US&"));
            Assert.Equal(expectedValue, lotteryBetType);
        }
    }
}