namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using Fanex.RemotingRef.NetCore;

    public class RefSyncControllerTestBase : ControllerTestBase
    {
        protected RefSyncController RefSyncController { get; set; }

        protected override void DisposeController()
            => RefSyncController.Dispose();
    }
}