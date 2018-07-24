namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using Fanex.RemotingRef.Service.Controllers;

    public class RefSyncControllerTestBase : ControllerTestBase
    {
        protected RefSyncControllerTestBase()
            => RefSyncController = new RefSyncController(RefData);

        protected RefSyncController RefSyncController { get; }

        protected override void DisposeController()
            => RefSyncController.Dispose();
    }
}