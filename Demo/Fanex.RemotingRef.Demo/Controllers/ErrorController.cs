using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{

    public class ErrorController : Controller
    {
        public ActionResult Unauthorized()
        {
            return View("~/Views/Shared/Unauthorized.cshtml");
        }
    }
}