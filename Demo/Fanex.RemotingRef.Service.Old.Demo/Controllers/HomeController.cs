namespace Fanex.RemotingRef.Service.Old.Demo.Controllers
{
    using System.Web.Mvc;
    using global::RemotingRef;
    using Swashbuckle.Swagger.Annotations;

    public class HomeController : Controller
    {
        /// <summary>
        /// Get bet type name
        /// </summary>
        /// <remarks>By passing in the appropriate options, you can get bet type name in multiple languages </remarks>
        /// <param name="betTypeId">Bet type id</param>
        /// <param name="languageCode">Default value is "en-US"</param>
        /// <returns>Bet Type Name in multiple languages</returns>
        [HttpGet]
        [SwaggerOperation("BetTypeName")]
        public ActionResult BetTypeName(string betTypeId, string languageCode = "en-US")
        {
            return Json(RefData.GetBetTypeName(betTypeId, languageCode));
        }

        [HttpGet]
        public ActionResult SportName(string sportTypeId, string languageCode = "en-US")
        {
            var result = RefData.GetSportName(sportTypeId, languageCode);

            return Json(result);
        }

        [HttpGet]
        public ActionResult LeagueName(string leagueId, string languageCode = "en-US")
        {
            var result = RefData.GetLeagueName(leagueId, languageCode);

            return Json(result);
        }

        [HttpGet]
        public ActionResult TeamName(string teamId, string languageCode = "en-US")
        {
            var result = RefData.GetTeamName(teamId, languageCode);

            return Json(result);
        }

        [HttpGet]
        public ActionResult ResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode = "en-US", string defaultValue = "")
        {
            var result = RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, defaultValue);

            return Json(result);
        }
    }
}