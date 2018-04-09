namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Attribute;

    [NapSuperAuth]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}