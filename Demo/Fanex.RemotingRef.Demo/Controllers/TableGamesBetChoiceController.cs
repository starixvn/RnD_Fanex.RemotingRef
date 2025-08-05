using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class TableGamesBetChoiceController : BaseController
    {
        public TableGamesBetChoiceController(IRemotingRefDataAccess remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult TableGamesBetChoice()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Table Games Bet Choice",
                TypeIdLabel = "Bet Type Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult TableGamesBetChoice(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Table Games Bet Choice",
                TypeIdLabel = "Bet Type Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetTableGamesBetChoice(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

    }
}