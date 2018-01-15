namespace Fanex.RemotingRef.Old.Demo.Controllers
{
    using global::RemotingRef;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class HomeController : Controller
    {
        [HttpGet]
        [Route("BetTypeName")]
        [SwaggerOperation("BetTypeName")]
        public IActionResult BetTypeName([FromQuery]string betTypeId, [FromQuery]string languageCode = "en-US")
        {
            var betTypeName = RefData.GetBetTypeName(betTypeId, languageCode);

            return Ok(betTypeName);
        }
    }
}