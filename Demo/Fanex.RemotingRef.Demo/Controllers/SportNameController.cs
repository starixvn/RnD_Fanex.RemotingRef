using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class SportNameController : BaseController
    {
        public SportNameController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult SportName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Sport Name",
                TypeIdLabel = "Sport Type Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SportName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Sport Name",
                TypeIdLabel = "Sport Type Id",
                TypeId = typeId ?? "1",
                Language = language,
                OurData = _remotingRefService.GetSportName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult SportNameList()
        {
            var sportTypeInputData = new[] { 1 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var sportTypeId in sportTypeInputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Sport Type",
                    TypeId = sportTypeId.ToString(CultureInfo.InvariantCulture),
                    OurData = _remotingRefService.GetSportName(sportTypeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}