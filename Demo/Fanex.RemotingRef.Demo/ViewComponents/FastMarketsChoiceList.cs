namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.DataAccesses;
    using Microsoft.AspNetCore.Mvc;

    public class FastMarketsChoiceList : ViewComponent
    {
        private readonly IRemotingRefDataAccess _remotingRefDataAccess;

        public FastMarketsChoiceList(IRemotingRefDataAccess remotingRefDataAccess)
        {
            _remotingRefDataAccess = remotingRefDataAccess;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var betTypeInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var betTeamInputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var handicap1InputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var handicap2InputData = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var viewModel = new List<BetTypeNameViewModel>();

            for (int i = 0; i < betTypeInputData.Length; i++)
            {
                var betTypeViewModel = new BetTypeNameViewModel
                {
                    DataType = "Bet Type",
                    TypeId = betTypeInputData[i].ToString(CultureInfo.InvariantCulture),
                    RefDataResult =
                        _remotingRefDataAccess.GetFastMarketsChoice(
                            betTypeInputData[i].ToString(CultureInfo.InvariantCulture),
                            betTeamInputData[i].ToString(CultureInfo.InvariantCulture),
                            handicap1InputData[i],
                            handicap2InputData[i])
                };

                viewModel.Add(betTypeViewModel);
            }

            return View("~/Views/Ref/_Share/RefResultList.cshtml", viewModel);
        }
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}