using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using System.Collections.Generic;

    public class ResourceDataController : BaseController
    {
        public ResourceDataController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
        {
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
                OurData = string.Empty
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
                OurData = _remotingRefService.GetResourceDataByRefIdAndResourceId(typeId, resourceId, language, string.Empty)
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
                    OurData = _remotingRefService.GetResourceDataByRefIdAndResourceId(input.RefId, input.ResourceId, "en-US", string.Empty)
                };

                viewModels.Add(viewModel);
            }

            return PartialView("~/Views/Ref/ResourceData/ResourceDataList.cshtml", viewModels);
        }
    }
}