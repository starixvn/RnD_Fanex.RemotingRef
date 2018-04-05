namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class CountryNameController : Controller
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public CountryNameController(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        [HttpGet]
        public IActionResult CountryName()
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "CountryName",
                TypeId = "3",
                Language = Common.DefaultLanguage,
                RefDataResult = new RefDataResult()
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult CountryName(string typeId, string language)
        {
            var viewModel = new RefDataViewModel
            {
                DataType = "CountryName",
                TypeId = typeId ?? "3",
                Language = language,
                RefDataResult = _remotingRefDataAccess.GetCountryName(typeId, language)
            };

            return View("~/Views/Ref/Common/Index.cshtml", viewModel);
        }
    }
}