namespace Fanex.RemotingRef.Service.Demo.Old.Controllers
{
    using System.Web.Http;
    using global::RemotingRef;

    public class RefController : ApiController
    {
        [HttpGet]
        public string BetTypeName([FromUri]string betTypeId, [FromUri] string languageCode = "en-US")
        {
            return RefData.GetBetTypeName(betTypeId, languageCode);
        }

        [HttpGet]
        public string SportName([FromUri]string sportTypeId, [FromUri] string languageCode = "en-US")
        {
            return RefData.GetSportName(sportTypeId, languageCode);
        }

        [HttpGet]
        public string TeamName([FromUri]string teamId, [FromUri] string languageCode = "en-US")
        {
            return RefData.GetTeamName(teamId, languageCode);
        }

        [HttpGet]
        public string LeagueName([FromUri]string leagueId, [FromUri] string languageCode = "en-US")
        {
            return RefData.GetLeagueName(leagueId, languageCode);
        }

        [HttpGet]
        public string ResourceDataByRefIdAndResourceId([FromUri]string refId, [FromUri]string resourceId, [FromUri] string languageCode = "en-US")
        {
            return RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, string.Empty);
        }
    }
}