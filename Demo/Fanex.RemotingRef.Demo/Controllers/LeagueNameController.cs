namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class LeagueNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public LeagueNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult LeagueName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "League Name",
                TypeIdLabel = "League Id",
                TypeId = "3",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult LeagueName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "League Name",
                TypeIdLabel = "League Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetLeagueName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult LeagueNameList()
        {
            var inputData = new[] { 3, 4, 5, 6, 7, 8, 10 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "League",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetLeagueName(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}