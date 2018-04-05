namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class SportNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public SportNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult SportName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "SportName",
                TypeId = "1",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult SportName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "SportName",
                TypeId = typeId ?? "1",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetSportName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}