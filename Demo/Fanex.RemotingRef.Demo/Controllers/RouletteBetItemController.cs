namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class RouletteBetItemController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public RouletteBetItemController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult RouletteBetItem()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "RouletteBetItem",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/RouletteBetItem/RouletteBetItem.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult RouletteBetItem(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "RouletteBetItem",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetRouletteBetItem(typeId, language)
            };

            return View("~/Views/Ref/RouletteBetItem/RouletteBetItem.cshtml", viewModel);
        }
    }
}