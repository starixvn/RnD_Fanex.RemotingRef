namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class SportNameList : ViewComponent
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public SportNameList(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sportTypeInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var sportTypeId in sportTypeInputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Sport Type",
                    TypeId = sportTypeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetSportName(sportTypeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return View("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }
}