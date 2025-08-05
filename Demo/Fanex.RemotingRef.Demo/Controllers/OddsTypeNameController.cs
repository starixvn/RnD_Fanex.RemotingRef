using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;
    using System.Globalization;

    public class OddsTypeNameController : BaseController
    {
        public OddsTypeNameController(IRemotingRefDataAccess remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult OddsTypeName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Odds Type Name",
                TypeIdLabel = "Odds Type Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult OddsTypeName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Odds Type Name",
                TypeIdLabel = "Odds Type Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetOddsTypeName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult OddsTypeNameList()
        {
            var inputData = new[] { "1", "2", "3", "4", "5" };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "OddsType",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetOddsTypeName(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return PartialView("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}