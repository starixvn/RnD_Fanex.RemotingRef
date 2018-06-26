namespace Fanex.RemotingRef.Service.Configs
{
    using System.Collections.Specialized;
    using System.Configuration;

    public static class AppSettings
    {
        internal static NameValueCollection Configs { get; set; } = ConfigurationManager.AppSettings;

        public static bool EnabledTrackingRequest => Configs["EnabledTrackingRequest"] == "1";
    }
}