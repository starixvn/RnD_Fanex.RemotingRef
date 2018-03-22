namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class RefResult : ViewComponent
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<IViewComponentResult> InvokeAsync(RefDataViewModel viewModel)
        {
            return View("~/Views/Ref/_Share/RefResult.cshtml", viewModel);
        }
    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}