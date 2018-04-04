namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class OddsTypeNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public OddsTypeNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult OddsTypeName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "OddsType",
                TypeId = "3",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/OddsTypeName/OddsTypeName.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult OddsTypeName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "OddsType",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetLeagueName(typeId, language)
            };

            return View("~/Views/Ref/OddsTypeName/OddsTypeName.cshtml", viewModel);
        }
    }
}