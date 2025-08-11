using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class LotteryBetChoiceController : BaseController
    {
        public LotteryBetChoiceController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult LotteryBetChoice()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Lottery Bet Choice",
                TypeIdLabel = "Bet Choice Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                OurData = string.Empty
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult LotteryBetChoice(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Lottery Bet Choice",
                TypeIdLabel = "Bet Choice Id",
                TypeId = typeId ?? "3",
                Language = language,
                OurData = $"Name: {_remotingRefService.GetLotteryBetChoice(typeId, language, string.Empty, out string id)} - Id: {id}" 
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult LotteryBetTypeList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "LotteryBetType",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    OurData = _remotingRefService.GetLotteryBetType(typeId.ToString(CultureInfo.InvariantCulture), "en-US")
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}