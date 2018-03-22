namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class TeamNameList : ViewComponent
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public TeamNameList(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var inputData = new[] { 5, 6, 7, 8, 10, 11, 12, 13, 14 };
            var viewModels = new List<RefDataViewModel>();

            foreach (var typeId in inputData)
            {
                var refDataViewModel = new RefDataViewModel
                {
                    DataType = "Team",
                    TypeId = typeId.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetTeamName(typeId.ToString(CultureInfo.InvariantCulture))
                };

                viewModels.Add(refDataViewModel);
            }

            return View("~/Views/Ref/_Share/RefResultList.cshtml", viewModels);
        }
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}