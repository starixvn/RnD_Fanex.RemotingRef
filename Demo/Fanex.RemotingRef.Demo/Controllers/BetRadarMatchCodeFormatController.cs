using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class BetRadarMatchCodeFormatController : BaseController
    {
        public BetRadarMatchCodeFormatController(IRemotingRefService remotingRefDataAccess)
            : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult BetRadarMatchCodeFormat()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Bet Radar Match Code Format",
                TypeIdLabel = "Match Code",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult BetRadarMatchCodeFormat(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Bet Radar Match Code Format",
                TypeIdLabel = "Match Code",
                TypeId = typeId ?? "3",
                Language = language,
                OurData = _remotingRefService.GetBetRadarMatchCodeFormat(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult BetRadarMatchCodeFormatList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Bet Radar Match Code Format",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    OurData = _remotingRefService.GetBetRadarMatchCodeFormat(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}