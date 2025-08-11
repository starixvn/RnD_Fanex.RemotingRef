using Microsoft.AspNetCore.Mvc;
namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public class FastMarketsResultController : BaseController
    {
        public FastMarketsResultController(IRemotingRefService remotingRefDataAccess) : base(remotingRefDataAccess)
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
                OurData = string.Empty
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
                OurData = _remotingRefService.GetFastMarketsResult(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}