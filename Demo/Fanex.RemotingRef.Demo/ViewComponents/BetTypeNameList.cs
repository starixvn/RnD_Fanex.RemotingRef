namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Microsoft.AspNetCore.Mvc;

    public class BetTypeNameList : ViewComponent
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public BetTypeNameList(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var betTypeInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var betTypeViewModels = new List<BetTypeNameViewModel>();

            foreach (var betType in betTypeInputData)
            {
                var betTypeViewModel = new BetTypeNameViewModel
                {
                    DataType = "Bet Type",
                    TypeId = betType.ToString(CultureInfo.InvariantCulture),
                    RefDataResult = _remotingRefDataAccess.GetBetTypeName(betType.ToString(CultureInfo.InvariantCulture))
                };

                betTypeViewModels.Add(betTypeViewModel);
            }

            return View("~/Views/Ref/_Share/RefResultList.cshtml", betTypeViewModels);
        }
    }
}