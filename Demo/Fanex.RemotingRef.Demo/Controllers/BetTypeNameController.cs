namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class BetTypeNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public BetTypeNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult BetTypeName()
        {
            var viewModel = new BetTypeNameViewModel
            {
                DataType = "Bet Type",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefNo = 0,
                ResourceId = Common.DefaultResourceId,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult BetTypeName(string typeId, long refNo, string resourceId, string language)
        {
            var viewModel = new BetTypeNameViewModel
            {
                IsFirstLoad = false,
                DataType = "Bet Type",
                TypeId = typeId ?? "1",
                Language = language,
                RefNo = refNo,
                ResourceId = resourceId ?? string.Empty,
                RefDataResult = _remotingRefDataAccess.GetBetTypeName(typeId, refNo, resourceId, language)
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult BetTypeNameList()
        {
            var betTypeInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var betTypeViewModels = new List<BetTypeNameViewModel>();

            foreach (var betType in betTypeInputData)
            {
                var betTypeViewModel = new BetTypeNameViewModel
                {
                    DataType = "Bet Type",
                    TypeId = betType.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture))
                };

                betTypeViewModels.Add(betTypeViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", betTypeViewModels);
        }
    }
}