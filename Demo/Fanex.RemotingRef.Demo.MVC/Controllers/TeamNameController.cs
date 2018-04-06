namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

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
                DataType = "TeamName",
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
                DataType = "TeamName",
                TypeId = typeId ?? "5",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetTeamName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}