namespace Fanex.RemotingRef.Service.Tests.Controllers.RefControllerTests
{
    using Fanex.RemotingRef.Service.Controllers;

    public class RefControllerTestBase : ControllerTestBase
    {
        protected RefControllerTestBase()
            => RefController = new RefController(RefData);

        protected RefController RefController { get; }

        protected override void DisposeController()
            => RefController.Dispose();
    }
}