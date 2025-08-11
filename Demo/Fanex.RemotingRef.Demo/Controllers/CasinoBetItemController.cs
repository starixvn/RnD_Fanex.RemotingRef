using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class CasinoBetItemController : BaseController
    {

        public CasinoBetItemController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult CasinoBetItem()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Casino Bet Item",
                TypeIdLabel = "Item Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CasinoBetItem(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Casino Bet Item",
                TypeIdLabel = "Item Id",
                TypeId = typeId ?? "3",
                Language = language,
                OurData = _remotingRefService.GetCasinoBetItem(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult CasinoBetItemList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Casino Bet Item",
                    TypeIdLabel = "Item Id",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    OurData = _remotingRefService.GetCasinoBetItem(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}