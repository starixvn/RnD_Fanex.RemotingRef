namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Web.Mvc;
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class CasinoBetItemController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public CasinoBetItemController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public ActionResult CasinoBetItem()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "CasinoBetItem",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult CasinoBetItem(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "CasinoBetItem",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetCasinoBetItem(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}