using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;


    public class FastMarkets225ResultController : BaseController
    {
        public FastMarkets225ResultController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
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
                OurData = string.Empty
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
                OurData = _remotingRefService.GetFastMarkets225Result(typeId, language)
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
                    OurData = _remotingRefService.GetFastMarkets225Result(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}