namespace Fanex.RemotingRef.Demo.BetTypeName
{
    using System.Threading.Tasks;
    using Fanex.RemotingRef.Demo.Models;
    using Microsoft.AspNetCore.Mvc;

    public class RefResult : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RefDataViewModel viewModel)
        {
            return View("~/Views/Ref/_Share/RefResult.cshtml", viewModel);
        }
    }
}