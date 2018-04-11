namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Filters;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class CasinoBetItemController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public CasinoBetItemController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
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
                RefDataResult = new RefDataResult()
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
                RefDataResult = _remotingRefDataAccess.GetCasinoBetItem(typeId, language)
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
                    RefDataResult = _remotingRefDataAccess.GetCasinoBetItem(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}