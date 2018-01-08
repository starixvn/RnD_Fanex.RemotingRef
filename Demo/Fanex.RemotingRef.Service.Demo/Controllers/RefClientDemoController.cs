namespace Fanex.RemotingRef.Service.Demo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// RemotingRef Client Demo
    /// </summary>
    public class RefClientDemoController : Controller
    {
        private readonly IRemotingRefService _remotingRefService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefClientDemoController"/> class.
        /// </summary>
        /// <param name="remotingRefService">remotingRefService</param>
        public RefClientDemoController(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        /// <summary>
        /// Get bet type name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="languageCode">Default value is "en-US"</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("BetTypeName")]
        [SwaggerOperation("BetTypeName")]
        public IActionResult BetTypeName([FromQuery]string betTypeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var betTypeName = _remotingRefService.GetBetTypeName(betTypeId, languageCode);

            return Ok(betTypeName);
        }

        /// <summary>
        /// Get bet type name with refNo
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="refNo">RefNo > 1000000000 used for showing (Via Lucky)</param>
        /// <param name="languageCode">Default value is "en-US"</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("BetTypeNameWithRefNo")]
        [SwaggerOperation("BetTypeNameWithRefNo")]
        public IActionResult BetTypeNameWithRefNo([FromQuery]string betTypeId, [FromQuery]long refNo, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var betTypeName = _remotingRefService.GetBetTypeName(betTypeId, languageCode, refNo);

            return Ok(betTypeName);
        }

        /// <summary>
        /// Get bet type name with resourceId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="resourceId">For showing additional data of bet type name. For example: "X" from "Set X Total Games" (bet type 1312)</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("BetTypeNameWithResourceId")]
        [SwaggerOperation("BetTypeName With ResourceId")]
        public IActionResult BetTypeNameWithResourceId([FromQuery]string betTypeId, [FromQuery]string resourceId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetBetTypeName(betTypeId, languageCode, string.Empty, resourceId);

            return Ok(result);
        }

        /// <summary>
        /// Get bet type name with resourceId and refNo
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="resourceId">For showing additional data of bet type name. For example: "X" from "Set X Total Games" (bet type 1312)</param>
        /// <param name="refNo">RefNo > 1000000000 used for showing (Via Lucky)</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("BetTypeNameWithRefNoAndResourceId")]
        [SwaggerOperation("BetTypeNameWithRefNoAndResourceId")]
        public IActionResult BetTypeNameWithRefNoAndResourceId(
            [FromQuery]string betTypeId, [FromQuery]string resourceId, [FromQuery]long refNo, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetBetTypeName(betTypeId, languageCode, string.Empty, resourceId, refNo);

            return Ok(result);
        }

        /// <summary>
        /// Get sport name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get sport name in multiple languages </remarks>
        /// <param name="sportTypeId">Sport type id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("SportName")]
        [SwaggerOperation("SportName")]
        public IActionResult SportName([FromQuery]string sportTypeId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetSportName(sportTypeId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get team name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get team name in multiple languages </remarks>
        /// <param name="teamId">Team id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("TeamName")]
        [SwaggerOperation("TeamName")]
        public IActionResult TeamName([FromQuery]string teamId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetTeamName(teamId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get team 3rd name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get team 3rd name in multiple languages </remarks>
        /// <param name="team3rdId">Team 3rd id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("TeamName3rd")]
        [SwaggerOperation("TeamName3rd")]
        public IActionResult TeamName3rd([FromQuery]string team3rdId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetTeamName3rd(team3rdId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get league name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get league name in multiple languages </remarks>
        /// <param name="leagueId">League id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("LeagueName")]
        [SwaggerOperation("LeagueName")]
        public IActionResult LeagueName([FromQuery]string leagueId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetLeagueName(leagueId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get league 3rd name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get league 3rd name in multiple languages </remarks>
        /// <param name="leagueId">League 3rd id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("LeagueName3rd")]
        [SwaggerOperation("LeagueName3rd")]
        public IActionResult LeagueName3rd([FromQuery]string leagueId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetLeagueName3rd(leagueId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get country name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get country name in multiple languages </remarks>
        /// <param name="countryId">Country id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("CountryName")]
        [SwaggerOperation("CountryName")]
        public IActionResult CountryName([FromQuery]string countryId, [FromQuery]string languageCode, string defaultValue = "")
        {
            var result = _remotingRefService.GetCountryName(countryId, languageCode, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get odds type name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get odds type name in multiple languages </remarks>
        /// <param name="oddsTypeId">Odds type id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("OddsTypeName")]
        [SwaggerOperation("OddsTypeName")]
        public IActionResult OddsTypeName([FromQuery]string oddsTypeId, [FromQuery]string languageCode)
        {
            var result = _remotingRefService.GetOddsTypeName(oddsTypeId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get ResourceDataByRefIdAndResourceId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get resource data in multiple languages </remarks>
        /// <param name="refId">ref id</param>
        /// <param name="resourceId">resource id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("ResourceDataByRefIdAndResourceId")]
        [SwaggerOperation("ResourceDataByRefIdAndResourceId")]
        public IActionResult ResourceDataByRefIdAndResourceId(
            [FromQuery]string refId,
            [FromQuery]string resourceId,
            [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage,
            [FromQuery] string defaultValue = "")
        {
            var result = _remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get ParentIdByBetTypeId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get resource data in multiple languages </remarks>
        /// <param name="betTypeId">bet type id</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("ParentIdByBetTypeId")]
        [SwaggerOperation("ParentIdByBetTypeId")]
        public IActionResult ParentIdByBetTypeId([FromQuery]string betTypeId, [FromQuery] string defaultValue = "")
        {
            var result = _remotingRefService.GetParentIdByBetTypeId(betTypeId, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get CombinationTypeCombo
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get combination type of combo parlay in multiple languages </remarks>
        /// <param name="betTeam">bet team</param>
        /// <param name="betCheck">bet check</param>
        /// <param name="handicap1">handicap1</param>
        /// <param name="handicap2">handicap2</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("CombinationTypeCombo")]
        [SwaggerOperation("CombinationTypeCombo")]
        public IActionResult CombinationTypeCombo([FromQuery]string betTeam, [FromQuery] string betCheck, [FromQuery] int handicap1, [FromQuery] int handicap2, [FromQuery] string languageCode)
        {
            var result = _remotingRefService.GetCombinationTypeCombo(betTeam, betCheck, handicap1, handicap2, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get HorseTeamName
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get horse team name in multiple languages </remarks>
        /// <param name="codeId">Horse team code id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("HorseTeamName")]
        [SwaggerOperation("HorseTeamName")]
        public IActionResult HorseTeamName([FromQuery]string codeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage, [FromQuery] string defaultValue = "")
        {
            var result = _remotingRefService.GetHorseTeamName(codeId, languageCode, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get CasinoBetItem
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get casino bet item in multiple languages </remarks>
        /// <param name="itemId">Casino item id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("CasinoBetItem")]
        [SwaggerOperation("CasinoBetItem")]
        public IActionResult CasinoBetItem([FromQuery]string itemId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage, [FromQuery] string defaultValue = "")
        {
            var result = _remotingRefService.GetCasinoBetItem(itemId, languageCode, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get RouletteBetItem
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get  Roulette bet item in multiple languages </remarks>
        /// <param name="itemId">Roulette item id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <param name="defaultValue">Default value is empty</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("RouletteBetItem")]
        [SwaggerOperation("RouletteBetItem")]
        public IActionResult RouletteBetItem([FromQuery]string itemId, [FromQuery]string languageCode, [FromQuery] string defaultValue = "")
        {
            var result = _remotingRefService.GetRouletteBetItem(itemId, languageCode, defaultValue);

            return Ok(result);
        }

        /// <summary>
        /// Get KenoBetChoice
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Keno bet choice name and bet type id in multiple languages </remarks>
        /// <param name="betChoiceId">Keno bet choice id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("KenoBetChoice")]
        [SwaggerOperation("KenoBetChoice")]
        public IActionResult KenoBetChoice([FromQuery]string betChoiceId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetKenoBetChoice(betChoiceId, languageCode, out string kenoBetTypeId);

            return Ok(new { result, kenoBetTypeId });
        }

        /// <summary>
        /// Get KenoBetType
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Keno bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Keno bet type id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("KenoBetType")]
        [SwaggerOperation("KenoBetType")]
        public IActionResult KenoBetType([FromQuery]string betTypeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetKenoBetType(betTypeId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get FastMarketsChoice
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get FastMarkets choices in multiple languages </remarks>
        /// <param name="betTypeId">bet team</param>
        /// <param name="betTeam">bet check</param>
        /// <param name="handicap1">handicap1</param>
        /// <param name="handicap2">handicap2</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("FastMarketsChoice")]
        [SwaggerOperation("FastMarketsChoice")]
        public IActionResult FastMarketsChoice(
            [FromQuery]string betTypeId,
            [FromQuery]string betTeam,
            [FromQuery] int handicap1,
            [FromQuery] int handicap2,
            [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, languageCode);
            return Ok(result);
        }

        /// <summary>
        /// Get FastMarkets225Result
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get FastMarkets result for bet type 225 in multiple languages </remarks>
        /// <param name="resultId">Result id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("FastMarkets225Result")]
        [SwaggerOperation("FastMarkets225Result")]
        public IActionResult FastMarkets225Result([FromQuery]string resultId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetFastMarkets225Result(resultId, languageCode);

            return Ok(result);
        }

        /// <summary>
        /// Get LotteryBetChoice
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Lottery bet choice name and bet type id in multiple languages </remarks>
        /// <param name="betChoiceId">Lottery bet choice id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("LotteryBetChoice")]
        [SwaggerOperation("LotteryBetChoice")]
        public IActionResult LotteryBetChoice([FromQuery]string betChoiceId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetLotteryBetChoice(betChoiceId, languageCode, out string betTypeId);

            return Ok(new { result, betTypeId });
        }

        /// <summary>
        /// Get LotteryBetTypeName
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Lottery bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Lottery bet type id</param>
        /// <param name="languageCode">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("LotteryBetTypeName")]
        [SwaggerOperation("LotteryBetTypeName")]
        public IActionResult LotteryBetTypeName([FromQuery]string betTypeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var result = _remotingRefService.GetLotteryBetType(betTypeId, languageCode);

            return Ok(result);
        }
    }
}