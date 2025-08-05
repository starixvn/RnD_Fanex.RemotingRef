using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class FastMarketsChoiceController : BaseController
    {
        public FastMarketsChoiceController(IRemotingRefDataAccess remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult FastMarketsChoice()
        {
            var viewModel = new FastMarketChoiceViewModel
            {
                DataType = "FastMarkets Choice",
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
                IsFirstLoad = false,
                DataType = "FastMarkets Choice",
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
        public ActionResult FastMarketsChoiceList()
        {
            var betTypeInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var betTeamInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var handicap1InputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var handicap2InputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var viewModel = new List<BetTypeNameViewModel>();

            for (int i = 0; i < betTypeInputData.Length; i++)
            {
                var betTypeViewModel = new BetTypeNameViewModel
                {
                    DataType = "FastMarketsChoice",
                    TypeId = betTypeInputData[i].ToString(CultureInfo.InvariantCulture),
                    RefDataResult =
                        _remotingRefDataAccess.GetFastMarketsChoice(
                            betTypeInputData[i].ToString(CultureInfo.InvariantCulture),
                            betTeamInputData[i].ToString(CultureInfo.InvariantCulture),
                            handicap1InputData[i],
                            handicap2InputData[i])
                };

                viewModel.Add(betTypeViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModel);
        }
    }
}