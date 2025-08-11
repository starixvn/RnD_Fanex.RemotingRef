using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public class TableGamesBetChoiceController : BaseController
    {
        public TableGamesBetChoiceController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
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
                OurData = string.Empty
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
                OurData = $"Name: {_remotingRefService.GetTableGamesBetChoice(typeId, language, out string id)} - id: {id}"
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

    }
}