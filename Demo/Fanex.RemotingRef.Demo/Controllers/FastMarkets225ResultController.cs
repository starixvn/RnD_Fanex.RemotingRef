namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class FastMarkets225ResultController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public FastMarkets225ResultController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult FastMarkets225Result()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Fast Markets 225 Result",
                TypeIdLabel = "Result Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult FastMarkets225Result(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Fast Markets 225 Result",
                TypeIdLabel = "Result Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetFastMarkets225Result(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult FastMarkets225ResultList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "FastMarket225Result",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetFastMarkets225Result(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}