namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Filters;
    using Fanex.RemotingRef.Demo.Models;

    [Authorize]
    public class DisplayedTypeIdController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public DisplayedTypeIdController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult DisplayedTypeId()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Displayed Type Id",
                TypeIdLabel = "Bet Type Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult DisplayedTypeId(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Displayed Type Id",
                TypeIdLabel = "Bet Type Id",
                TypeId = typeId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetParentIdByBetTypeId(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult DisplayedTypeIdList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Displayed Type",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetParentIdByBetTypeId(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}