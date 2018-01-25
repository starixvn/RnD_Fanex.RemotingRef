namespace Fanex.RemotingRef.Service.Demo.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// RemotingRef Client Demo
    /// </summary>
    public class RefClientDemoController : Controller
    {
        private readonly IRemotingRefService _remotingRefService;
        private readonly ILogger<RefClientDemoController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefClientDemoController"/> class.
        /// </summary>
        /// <param name="remotingRefService">remotingRefService</param>
        public RefClientDemoController(IRemotingRefService remotingRefService, ILogger<RefClientDemoController> logger)
        {
            _remotingRefService = remotingRefService;
            _logger = logger;
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
        public IActionResult BetTypeName([FromQuery]string betTypeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var betTypeName = string.Empty;

            try
            {
                betTypeName = _remotingRefService.GetBetTypeName(betTypeId, languageCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _logger.LogError(e.StackTrace);
            }

            return Ok(betTypeName);
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
        public IActionResult SportName([FromQuery]string sportTypeId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var sportName = string.Empty;

            try
            {
                sportName = _remotingRefService.GetSportName(sportTypeId, languageCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _logger.LogError(e.StackTrace);
            }

            return Ok(sportName);
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
        public IActionResult TeamName([FromQuery]string teamId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var teamName = string.Empty;

            try
            {
                teamName = _remotingRefService.GetTeamName(teamId, languageCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _logger.LogError(e.StackTrace);
            }

            return Ok(teamName);
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
        public IActionResult LeagueName([FromQuery]string leagueId, [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage)
        {
            var leagueName = string.Empty;

            try
            {
                leagueName = _remotingRefService.GetLeagueName(leagueId, languageCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _logger.LogError(e.StackTrace);
            }

            return Ok(leagueName);
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
        public IActionResult ResourceDataByRefIdAndResourceId(
            [FromQuery]string refId,
            [FromQuery]string resourceId,
            [FromQuery]string languageCode = RefDataTypeHandlerOptions.DefaultLanguage,
            [FromQuery] string defaultValue = "")
        {
            var resourceData = string.Empty;

            try
            {
                resourceData = _remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, defaultValue);
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.Message);
                _logger.LogError(e.StackTrace);
            }

            return Ok(resourceData);
        }
    }
}