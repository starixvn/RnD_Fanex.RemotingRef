namespace Fanex.RemotingRef.Service.Delegates
{
    using Fanex.RemotingRef.Service.Models;

    internal delegate T SyncTranslationDelegate<out T>(ParameterSet parameterSet, string refLanguageCode);
}