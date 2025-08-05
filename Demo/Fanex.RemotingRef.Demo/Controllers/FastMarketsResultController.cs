using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;

    public class FastMarketsResultController : BaseController
    {
        public FastMarketsResultController(IRemotingRefDataAccess remotingRefDataAccess) : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult FastMarketsResult()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Fast Markets Result",
                TypeIdLabel = "Result Id",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult FastMarketsResult(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Fast Markets  Result",
                TypeIdLabel = "Result Id",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetFastMarketsResult(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}