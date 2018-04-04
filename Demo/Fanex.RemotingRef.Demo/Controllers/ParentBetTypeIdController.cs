namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class ParentBetTypeIdController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public ParentBetTypeIdController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult ParentBetTypeId()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Parent Bet Type",
                TypeId = "3",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/ParentBetTypeId/ParentBetTypeId.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult ParentBetTypeId(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "Parent Bet Type",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetParentIdByBetTypeId(typeId, language)
            };

            return View("~/Views/Ref/ParentBetTypeId/ParentBetTypeId.cshtml", viewModel);
        }
    }
}