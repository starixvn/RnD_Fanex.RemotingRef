namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class ResourceDataController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public ResourceDataController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult ResourceData()
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
        public IActionResult ResourceData(string typeId, string resourceId, string language)
        {
            var viewModel = new ResourceDataViewModel
            {
                DataType = "Ref",
                TypeId = typeId ?? "AG_Baccarat",
                ResourceId = resourceId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetResourceData(typeId, resourceId, language)
            };

            return View("~/Views/Ref/ResourceData/ResourceData.cshtml", viewModel);
        }
    }
}