using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public class KenoBetChoiceController : BaseController
    {
        public KenoBetChoiceController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult KenoBetChoice()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Keno Bet Choice",
                TypeIdLabel = "Bet Choice Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult KenoBetChoice(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Keno Bet Choice",
                TypeIdLabel = "Bet Choice Id",
                TypeId = typeId ?? "1",
                Language = language,
                OurData = $"Name: {_remotingRefService.GetKenoBetChoice(typeId, language, out string id)} - Id: {id}"
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}