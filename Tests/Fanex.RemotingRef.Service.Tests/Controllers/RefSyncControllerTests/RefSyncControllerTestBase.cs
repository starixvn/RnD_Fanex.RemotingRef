namespace Fanex.RemotingRef.Service.Tests.Controllers.RefSyncControllerTests
{
    using Fanex.RemotingRef.NetCore;

    public class RefSyncControllerTestBase : ControllerTestBase
    {
        protected RefSyncControllerTestBase()
            => RefSyncController = new RefSyncController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

        protected RefSyncController RefSyncController { get; }

        protected override void DisposeController()
            => RefSyncController.Dispose();
    }
}