namespace Fanex.RemotingRef
{
    using Fanex.RemotingRef.TranslationStorage;

    internal interface IRefDataTypeHandler
    {
        RefDataTypeHandlerOptions Options { get; }

        object Get(KeySet keySet, string languageCode);
    }
}