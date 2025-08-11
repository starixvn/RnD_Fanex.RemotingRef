using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class TeamNameController : BaseController
    {
        public TeamNameController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
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
                OurData = string.Empty
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
                OurData = _remotingRefService.GetTeamName(typeId, language)
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
                    OurData = _remotingRefService.GetTeamName(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}