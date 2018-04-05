namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class LeagueNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public LeagueNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult LeagueName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "LeagueName",
                TypeId = "3",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult LeagueName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "LeagueName",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetLeagueName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}