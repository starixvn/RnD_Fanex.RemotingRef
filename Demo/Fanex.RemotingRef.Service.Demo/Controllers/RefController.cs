namespace Fanex.RemotingRef.Service.Demo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Ref Data Controller
    /// </summary>
    public class RefController : Controller
    {
        private readonly ISerializer _serializer;
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefController"/> class.
        /// </summary>
        public RefController()
        {
            _client = new HttpClient();
            _serializer = new MessagePackSerializer();
        }

        /// <summary>
        /// Get bet type name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/bettypename")]
        [SwaggerOperation("BetTypeName")]
        public IActionResult BetTypeName([FromQuery]string betTypeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("bettypename", new Dictionary<string, string> { { "betTypeId", betTypeId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get bet type name with refNo
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="refNo">RefNo > 1000000000 used for showing (Via Lucky)</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/bettypenamewithrefno")]
        [SwaggerOperation("BetTypeName With RefNo")]
        public IActionResult BetTypeNameWithRefNo([FromQuery]string betTypeId, [FromQuery]string refNo, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>(
                "bettypenamewithrefno",
                new Dictionary<string, string> { { "betTypeId", betTypeId }, { "refno", refNo } },
                languages);

            return Ok(result);
        }

        /// <summary>
        /// Get bet type name with resourceId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="resourceId">For showing additional data of bet type name. For example: "X" from "Set X Total Games" (bet type 1312)</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/bettypenamewithresourceid")]
        [SwaggerOperation("BetTypeName With RefNo")]
        public IActionResult BetTypeNameWithResourceId([FromQuery]string betTypeId, [FromQuery]string resourceId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>(
                "bettypenamewithresourceid",
                new Dictionary<string, string> { { "betTypeId", betTypeId }, { "resourceId", resourceId } },
                languages);

            return Ok(result);
        }

        /// <summary>
        /// Get bet type name with refNo and resourceId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="resourceId">For showing additional data of bet type name. For example: "X" from "Set X Total Games" (bet type 1312)</param>
        /// <param name="refNo">RefNo > 1000000000 used for showing (Via Lucky)</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/bettypenamewithrefnoandresourceid")]
        [SwaggerOperation("BetTypeName With RefNo")]
        public IActionResult BetTypeNameWithRefNoAndResourceId([FromQuery]string betTypeId, [FromQuery]string resourceId, [FromQuery]string refNo, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>(
                "bettypenamewithrefnoandresourceid",
                new Dictionary<string, string> { { "betTypeId", betTypeId }, { "resourceId", resourceId } },
                languages);

            return Ok(result);
        }

        /// <summary>
        /// Get sport name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get sport name in multiple languages </remarks>
        /// <param name="sportTypeId">Sport type id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/sportname")]
        [SwaggerOperation("SportName")]
        public IActionResult SportName([FromQuery]string sportTypeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("sportname", new Dictionary<string, string> { { "sportTypeId", sportTypeId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get team name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get team name in multiple languages </remarks>
        /// <param name="teamId">Team id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/teamname")]
        [SwaggerOperation("TeamName")]
        public IActionResult TeamName([FromQuery]string teamId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("teamname", new Dictionary<string, string> { { "teamId", teamId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get team 3rd name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get team 3rd name in multiple languages </remarks>
        /// <param name="team3rdId">Team 3rd id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/teamname3rd")]
        [SwaggerOperation("TeamName3rd")]
        public IActionResult TeamName3rd([FromQuery]string team3rdId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("teamname3rd", new Dictionary<string, string> { { "team3rdId", team3rdId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get league name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get league name in multiple languages </remarks>
        /// <param name="leagueId">League id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/leaguename")]
        [SwaggerOperation("TeamName")]
        public IActionResult LeagueName([FromQuery]string leagueId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("leaguename", new Dictionary<string, string> { { "leagueId", leagueId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get league 3rd name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get league 3rd name in multiple languages </remarks>
        /// <param name="leagueId">League 3rd id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/leaguename3rd")]
        [SwaggerOperation("LeagueName3rd")]
        public IActionResult LeagueName3rd([FromQuery]string leagueId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("leaguename3rd", new Dictionary<string, string> { { "leagueId", leagueId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get country name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get country name in multiple languages </remarks>
        /// <param name="countryId">Country id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/countryname")]
        [SwaggerOperation("CountryName")]
        public IActionResult CountryName([FromQuery]string countryId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("countryname", new Dictionary<string, string> { { "countryId", countryId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get odds type name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get odds type name in multiple languages </remarks>
        /// <param name="oddsTypeId">Odds type id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/oddstypename")]
        [SwaggerOperation("OddsTypeName")]
        public IActionResult OddsTypeName([FromQuery]string oddsTypeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("oddstypename", new Dictionary<string, string> { { "oddsTypeId", oddsTypeId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get resource data
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get resource data in multiple languages </remarks>
        /// <param name="refId">ref id</param>
        /// <param name="resourceId">resource id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/resourcedata")]
        [SwaggerOperation("ResourceData")]
        public IActionResult ResourceData([FromQuery]string refId, [FromQuery]string resourceId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("resourcedata", new Dictionary<string, string> { { "refId", refId }, { "resourceId", resourceId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get ParentIdByBetTypeId
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get resource data in multiple languages </remarks>
        /// <param name="betTypeId">bet type id</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/parentidbybettypeid")]
        [SwaggerOperation("ParentIdByBetTypeId")]
        public IActionResult ParentIdByBetTypeId([FromQuery]string betTypeId)
        {
            var result = RequestTranslation<string>("parentidbybettypeid", new Dictionary<string, string> { { "betTypeId", betTypeId } }, null);

            return Ok(result);
        }

#pragma warning disable SA1118 // Parameter must not span multiple lines

        /// <summary>
        /// Get CombinationTypeComboParlay
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get combination type of combo parlay in multiple languages </remarks>
        /// <param name="betTeam">bet team</param>
        /// <param name="betCheck">bet check</param>
        /// <param name="handicap1">handicap1</param>
        /// <param name="handicap2">handicap2</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/combinationtypecomboparlay")]
        [SwaggerOperation("CombinationTypeComboParlay")]
        public IActionResult CombinationTypeComboParlay([FromQuery]string betTeam, [FromQuery] string betCheck, [FromQuery] int handicap1, [FromQuery] int handicap2, [FromQuery] string[] languages)
        {
            var result = RequestTranslation<string[]>(
                "CombinationTypeComboParlay",
                new Dictionary<string, string>
                {
                    { "betTeam", betTeam },
                    { "betCheck", betCheck },
                    { "handicap1", handicap1.ToString(CultureInfo.InvariantCulture) },
                    { "handicap2", handicap2.ToString(CultureInfo.InvariantCulture) }
                },
                languages);

            return Ok(result);
        }

        /// <summary>
        /// Get HorseTeamName
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get horse team name in multiple languages </remarks>
        /// <param name="codeId">Horse team code id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/horseteamname")]
        [SwaggerOperation("HorseTeamName")]
        public IActionResult HorseTeamName([FromQuery]string codeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("horseteamname", new Dictionary<string, string> { { "codeId", codeId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get CasinoBetItem
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get casino bet item in multiple languages </remarks>
        /// <param name="itemId">Casino item id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/casinobetitem")]
        [SwaggerOperation("CasinoBetItem")]
        public IActionResult CasinoBetItem([FromQuery]string itemId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("CasinoBetItem", new Dictionary<string, string> { { "itemId", itemId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get RouletteBetItem
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get  Roulette bet item in multiple languages </remarks>
        /// <param name="itemId">Roulette item id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/roulettebetitem")]
        [SwaggerOperation("RouletteBetItem")]
        public IActionResult RouletteBetItem([FromQuery]string itemId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("RouletteBetItem", new Dictionary<string, string> { { "itemId", itemId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get KenoBetChoice
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Keno bet choice name and bet type id in multiple languages </remarks>
        /// <param name="betChoiceId">Keno bet choice id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/kenobetchoice")]
        [SwaggerOperation("KenoBetChoice")]
        public IActionResult KenoBetChoice([FromQuery]string betChoiceId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<Tuple<string, string>>("KenoBetChoice", new Dictionary<string, string> { { "betChoiceId", betChoiceId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get KenoBetType
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Keno bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Keno bet type id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/kenobettype")]
        [SwaggerOperation("KenoBetType")]
        public IActionResult KenoBetType([FromQuery]string betTypeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("KenoBetType", new Dictionary<string, string> { { "betTypeId", betTypeId } }, languages);

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
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/fastmarketschoice")]
        [SwaggerOperation("FastMarketsChoice")]
        public IActionResult FastMarketsChoice([FromQuery]string betTypeId, [FromQuery] string betTeam, [FromQuery] int handicap1, [FromQuery] int handicap2, [FromQuery] string[] languages)
        {
            var result = RequestTranslation<string>(
                "FastMarketsChoice",
                new Dictionary<string, string>
                {
                    { "betTypeId", betTypeId },
                    { "betTeam", betTeam },
                    { "handicap1", handicap1.ToString(CultureInfo.InvariantCulture) },
                    { "handicap2", handicap2.ToString(CultureInfo.InvariantCulture) }
                },
                languages);

            return Ok(result);
        }

        /// <summary>
        /// Get FastMarkets225Result
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get FastMarkets result for bet type 225 in multiple languages </remarks>
        /// <param name="resultId">Result id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/fastmarkets225result")]
        [SwaggerOperation("FastMarkets225Result")]
        public IActionResult FastMarkets225Result([FromQuery]string resultId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("FastMarkets225Result", new Dictionary<string, string> { { "resultId", resultId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get LotteryBetChoice
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Lottery bet choice name and bet type id in multiple languages </remarks>
        /// <param name="betChoiceId">Lottery bet choice id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/lotterybetchoice")]
        [SwaggerOperation("LotteryBetChoice")]
        public IActionResult LotteryBetChoice([FromQuery]string betChoiceId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<Tuple<string, string>>("LotteryBetChoice", new Dictionary<string, string> { { "betChoiceId", betChoiceId } }, languages);

            return Ok(result);
        }

        /// <summary>
        /// Get LotteryBetTypeName
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get Lottery bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Lottery bet type id</param>
        /// <param name="languages">Default value is ["en-US"]</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [Route("/api/ref/lotterybettypename")]
        [SwaggerOperation("LotteryBetTypeName")]
        public IActionResult LotteryBetTypeName([FromQuery]string betTypeId, [FromQuery]string[] languages)
        {
            var result = RequestTranslation<string>("LotteryBetTypeName", new Dictionary<string, string> { { "betTypeId", betTypeId } }, languages);

            return Ok(result);
        }

#pragma warning restore SA1118 // Parameter must not span multiple lines

        private Dictionary<string, T> RequestTranslation<T>(string path, Dictionary<string, string> parameters, string[] languages)
        {
            var result = new Dictionary<string, T>();
            var requestUri = new StringBuilder("http://localhost:65085/api/ref/").Append(path).Append("?");
            var defaultLanguages = (languages == null || languages.Length == 0) ? new[] { "en-US" } : languages;

            foreach (var parameter in parameters)
            {
                requestUri.Append($"{parameter.Key}={parameter.Value}&");
            }

            foreach (var language in defaultLanguages)
            {
                requestUri.Append($"languages={language}&");
            }

            var response = _client.GetAsync(new Uri(requestUri.ToString())).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseResult = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);

                result = _serializer.Deserialize<Dictionary<string, T>>(Convert.FromBase64String(responseResult));
            }

            return result;
        }
    }
}