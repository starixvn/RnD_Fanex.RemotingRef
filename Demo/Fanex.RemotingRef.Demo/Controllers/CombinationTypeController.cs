namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class CombinationTypeController : BaseController
    {
        public CombinationTypeController(IRemotingRefService remotingRefDataAccess)
            : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult CombinationType()
        {
            var viewModel = new CombinationTypeViewModel
            {
                DataType = "Combination Type",
                BetTeam = "1",
                BetCheck = "1",
                Hdp1 = 1,
                Hdp2 = 1,
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/CombinationType/CombinationType.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CombinationType(string betteam, string betcheck, int hdp1, int hdp2, string language)
        {
            var data = _remotingRefService.GetCombinationTypeCombo(betteam, betcheck, hdp1, hdp2, language);
            var viewModel = new CombinationTypeViewModel
            {
                IsFirstLoad = false,
                DataType = "Combination Type",
                BetTeam = betteam,
                BetCheck = betcheck,
                Hdp1 = hdp1,
                Hdp2 = hdp2,
                Language = language,
                OurData = string.Join(", ", data ?? Array.Empty<object>()),
            };

            return View("~/Views/Ref/CombinationType/CombinationType.cshtml", viewModel);
        }
    }
}