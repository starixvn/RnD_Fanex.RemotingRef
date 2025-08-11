namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using Fanex.RemotingRef.NetCore.Controllers;

    public class RefControllerTestBase : ControllerTestBase
    {
        protected RefController RefController { get; set; }

        protected override void DisposeController()
            => RefController.Dispose();
    }
}