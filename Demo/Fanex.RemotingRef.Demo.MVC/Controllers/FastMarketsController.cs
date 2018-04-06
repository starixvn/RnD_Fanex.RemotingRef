namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class FastMarketsController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public FastMarketsController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult FastMarketsChoice()
        {
            var viewModel = new FastMarketChoiceViewModel
            {
                DataType = "FastMarketsChoice",
                TypeId = "1",
                BetTeam = "1",
                Handicap1 = 1,
                Handicap2 = 2,
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/FastMarkets/FastMarketsChoice.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult FastMarketsChoice(string typeId, string betTeam, int handicap1, int handicap2, string language)
        {
            var viewModel = new FastMarketChoiceViewModel
            {
                DataType = "FastMarketsChoice",
                TypeId = typeId ?? "1",
                BetTeam = betTeam,
                Handicap1 = handicap1,
                Handicap2 = handicap2,
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetFastMarketsChoice(typeId, betTeam, handicap1, handicap2, language)
            };

            return View("~/Views/Ref/FastMarkets/FastMarketsChoice.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult FastMarkets225Result()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "FastMarkets225Result",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult FastMarkets225Result(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "FastMarkets225Result",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetFastMarkets225Result(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}