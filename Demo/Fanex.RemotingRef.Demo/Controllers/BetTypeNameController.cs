namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class BetTypeNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public BetTypeNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult BetTypeName()
        {
            var viewModel = new BetTypeNameViewModel
            {
                DataType = "Bet Type",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefNo = 0,
                ResourceId = Common.DefaultResourceId,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult BetTypeName(string typeId, long refNo, string resourceId, string language)
        {
            var viewModel = new BetTypeNameViewModel
            {
                DataType = "Bet Type",
                TypeId = typeId ?? "1",
                Language = language,
                RefNo = refNo,
                ResourceId = resourceId ?? string.Empty,
                RefDataResult = _remotingRefDataAccess.GetBetTypeName(typeId, refNo, resourceId, language)
            };

            return View("~/Views/Ref/BetTypeName/BetTypeName.cshtml", viewModel);
        }
    }
}