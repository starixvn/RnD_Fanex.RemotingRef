namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class OddsTypeNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public OddsTypeNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult OddsTypeName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "OddsTypeName",
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
                DataType = "OddsTypeName",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetOddsTypeName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}