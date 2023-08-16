namespace Fanex.RemotingRef.NetCore.Delegates
{
    using Fanex.RemotingRef.NetCore.Models;

    internal delegate T SyncNeutralResouceDelegate<out T>(ParameterSet parameterSet);
}