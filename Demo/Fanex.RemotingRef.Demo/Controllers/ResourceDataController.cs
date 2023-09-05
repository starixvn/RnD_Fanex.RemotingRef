using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using System.Collections.Generic;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class ResourceDataController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public ResourceDataController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult ResourceData()
        {
            var viewModel = new ResourceDataViewModel
            {
                DataType = "Ref",
                TypeId = "AG_Baccarat",
                ResourceId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/ResourceData/ResourceData.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult ResourceData(string typeId, string resourceId, string language)
        {
            var viewModel = new ResourceDataViewModel
            {
                IsFirstLoad = false,
                DataType = "Ref",
                TypeId = typeId ?? "AG_Baccarat",
                ResourceId = resourceId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetResourceData(typeId, resourceId, language)
            };

            return View("~/Views/Ref/ResourceData/ResourceData.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ResourceDataList()
        {
            var inputData = new[]
            {
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "1" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "11" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "12" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "13" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "2" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "3" },
                new ResourceDataInput { RefId = "AG_Baccarat", ResourceId = "4" },
            };

            var viewModels = new List<ResourceDataViewModel>();

            foreach (var input in inputData)
            {
                var viewModel = new ResourceDataViewModel
                {
                    DataType = "Ref",
                    TypeId = input.RefId,
                    ResourceId = input.ResourceId,
                    RefDataResult = _remotingRefDataAccess.GetResourceData(input.RefId, input.ResourceId)
                };

                viewModels.Add(viewModel);
            }

            return PartialView("~/Views/Ref/ResourceData/ResourceDataList.cshtml", viewModels);
        }
    }
}