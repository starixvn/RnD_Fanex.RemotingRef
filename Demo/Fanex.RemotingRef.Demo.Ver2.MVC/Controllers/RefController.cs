namespace Fanex.RemotingRef.Demo.Ver2.MVC.Controllers
{
    using System.Web.Http;

    public class RefController : ApiController
    {
        private readonly IRemotingRefService remotingRefService;

        public RefController()
        {
            remotingRefService = new RemotingRefService();
        }

        [HttpGet]
        public string BetTypeName([FromUri]string betTypeId, [FromUri] string languageCode = "en-US")
        {
            return remotingRefService.GetBetTypeName(betTypeId, languageCode);
        }

        [HttpGet]
        public string SportName([FromUri]string sportTypeId, [FromUri] string languageCode = "en-US")
        {
            return remotingRefService.GetSportName(sportTypeId, languageCode);
        }

        [HttpGet]
        public string TeamName([FromUri]string teamId, [FromUri] string languageCode = "en-US")
        {
            return remotingRefService.GetTeamName(teamId, languageCode);
        }

        [HttpGet]
        public string LeagueName([FromUri]string leagueId, [FromUri] string languageCode = "en-US")
        {
            return remotingRefService.GetLeagueName(leagueId, languageCode);
        }

        [HttpGet]
        public string ResourceDataByRefIdAndResourceId([FromUri]string refId, [FromUri]string resourceId, [FromUri] string languageCode = "en-US")
        {
            return remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, string.Empty);
        }
    }
}