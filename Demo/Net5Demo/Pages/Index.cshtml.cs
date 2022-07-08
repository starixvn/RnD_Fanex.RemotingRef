using Fanex.RemotingRef;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Net5Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRemotingRefService service;

        public string BetType1 { get; set; }

        public IndexModel(IRemotingRefService service)
        {
            this.service = service;
        }

        public void OnGet()
        {
            BetType1 = service.GetBetTypeName("1", "en-US");
        }
    }
}
