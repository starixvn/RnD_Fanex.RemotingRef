namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Filters;

    [RefAuth]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}