namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class KenoBetTypeController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public KenoBetTypeController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult KenoBetType()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Keno Bet Type",
                TypeIdLabel = "Bet Type Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult KenoBetType(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Keno Bet Type",
                TypeIdLabel = "Bet Type Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetKenoBetType(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult KenoBetTypeList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "KenoBetType",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetKenoBetType(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}