using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class HorseTeamNameController : BaseController
    {
        public HorseTeamNameController(IRemotingRefDataAccess remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult HorseTeamName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Horse Team Name",
                TypeIdLabel = "Code Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult HorseTeamName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Horse Team Name",
                TypeIdLabel = "Horse Team Id",
                TypeId = typeId ?? "5",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetHorseTeamName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult HorseTeamNameList()
        {
            var inputData = new[] { 1 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "HorseTeam",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetHorseTeamName(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}