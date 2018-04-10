namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        public ActionResult Unauthorized()
        {
            return View("~/Views/Shared/Unauthorized.cshtml");
        }
    }
}