namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class KenoBetTypeController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public KenoBetTypeController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult KenoBetType()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "KenoBetType",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult KenoBetType(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "KenoBetType",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetKenoBetType(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public IActionResult LotteryBetType()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "LotteryBetType",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult LotteryBetType(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "LotteryBetType",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetLotteryBetType(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}