namespace Fanex.RemotingRef.Demo.Controllers
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;
    using PublicEnum.Language;
    using RefDataModelStandard.Models.Request;

    public class BetTypeNameController : BaseController
    {
        public BetTypeNameController(IRemotingRefService remotingRefDataAccess)
            : base(remotingRefDataAccess)
        {
        }

        [HttpGet]
        public ActionResult BetTypeName()
        {
            var viewModel = new BetTypeNameViewModel
            {
                DataType = "Bet Type",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefNo = 0,
                ResourceId = Common.DefaultResourceId,
                OurData = string.Empty
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult BetTypeName(string typeId, long refNo, string resourceId, string language)
        {
            var viewModel = new BetTypeNameViewModel
            {
                IsFirstLoad = false,
                DataType = "Bet Type",
                TypeId = typeId ?? "1",
                Language = language,
                RefNo = refNo,
                ResourceId = resourceId ?? string.Empty,
                OurData = _remotingRefService.GetBetTypeName(typeId, language, string.Empty, resourceId, refNo),
                TWData = twRemotingRefService.GetBetTypeName(GetBetTypeNameModel.CreateModel(
                    int.Parse(typeId),
                    language.ConvertCultureToEnum(),
                    resourceId))
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }
    }
}