namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Web.Mvc;
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