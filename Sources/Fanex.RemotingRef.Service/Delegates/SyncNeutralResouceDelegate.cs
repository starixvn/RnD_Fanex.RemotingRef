namespace Fanex.RemotingRef.Service.Delegates
{
    using Fanex.RemotingRef.Service.Models;

    internal delegate T SyncNeutralResouceDelegate<out T>(ParameterSet parameterSet);
}