namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class TeamNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public TeamNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult TeamName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Team",
                TypeId = "5",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/TeamName/TeamName.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult TeamName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Team",
                TypeId = typeId ?? "5",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetTeamName(typeId, language)
            };

            return View("~/Views/Ref/TeamName/TeamName.cshtml", viewModel);
        }
    }
}