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
    public class TeamNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public TeamNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult TeamName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Team Name",
                TypeIdLabel = "Team Id",
                TypeId = "5",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult TeamName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Team Name",
                TypeIdLabel = "Team Id",
                TypeId = typeId ?? "5",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetTeamName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult TeamNameList()
        {
            var inputData = new[] { 5, 6, 7, 8 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Team",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetTeamName(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}