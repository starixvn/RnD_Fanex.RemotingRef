using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{    
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // Redirect về BetTypeName controller và action
            return RedirectToAction("BetTypeName", "BetTypeName");
        }
    }
}