namespace Fanex.RemotingRef.Demo.App_Start
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/style/common").Include("~/Content/site.min.css"));
        }
    }
}