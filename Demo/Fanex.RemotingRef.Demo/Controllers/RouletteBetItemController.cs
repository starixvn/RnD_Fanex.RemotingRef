using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class RouletteBetItemController : BaseController
    {
        public RouletteBetItemController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult RouletteBetItem()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Roulette Bet Item",
                TypeIdLabel = "Item Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult RouletteBetItem(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                TypeIdLabel = "Item Id",
                DataType = "Roulette Bet Item",
                TypeId = typeId ?? "3",
                Language = language,
                OurData = _remotingRefService.GetRouletteBetItem(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult RouletteBetItemList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "RouletteBetItem",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    OurData = _remotingRefService.GetRouletteBetItem(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}