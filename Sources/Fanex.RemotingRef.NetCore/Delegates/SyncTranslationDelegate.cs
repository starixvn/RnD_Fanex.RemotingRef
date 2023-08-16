namespace Fanex.RemotingRef.NetCore.Delegates
{
    using Fanex.RemotingRef.NetCore.Models;

    internal delegate T SyncTranslationDelegate<out T>(ParameterSet parameterSet, string refLanguageCode);
}