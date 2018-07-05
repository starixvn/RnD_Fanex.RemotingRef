namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class TeamName3rdController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public TeamName3rdController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult TeamName3rd()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Team Name 3rd",
                TypeIdLabel = "Team Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult TeamName3rd(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Team Name 3rd",
                TypeIdLabel = "Team Id",
                TypeId = typeId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetTeamName3rd(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult TeamName3rdList()
        {
            var inputData = new[] { 1 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Team3rd",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetTeamName3rd(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}