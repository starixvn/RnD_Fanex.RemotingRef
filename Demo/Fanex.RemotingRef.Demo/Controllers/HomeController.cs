namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Diagnostics;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}