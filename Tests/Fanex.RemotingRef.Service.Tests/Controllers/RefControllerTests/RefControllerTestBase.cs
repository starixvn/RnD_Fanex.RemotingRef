namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using Fanex.RemotingRef.NetCore.Controllers;

    public class RefControllerTestBase : ControllerTestBase
    {
        protected RefControllerTestBase()
            => RefController = new RefController(RefData, Team3rdService, League3rdService, BettypeService, SportService, RefResourceService, AppSettings);

        protected RefController RefController { get; }

        protected override void DisposeController()
            => RefController.Dispose();
    }
}