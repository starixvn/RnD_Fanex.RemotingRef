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
    public class SportNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public SportNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult SportName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "SportName",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult SportName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "SportName",
                TypeId = typeId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetSportName(typeId, language)
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
                    RefDataResult = _remotingRefDataAccess.GetSportName(sportTypeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}