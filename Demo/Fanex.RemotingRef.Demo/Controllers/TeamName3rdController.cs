using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class TeamName3rdController : BaseController
    {
        public TeamName3rdController(IRemotingRefService remotingRefDataAccess): base(remotingRefDataAccess)
        {
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
                OurData = string.Empty
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
                OurData = _remotingRefService.GetTeamName3rd(typeId, language)
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
                    OurData = _remotingRefService.GetTeamName3rd(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}