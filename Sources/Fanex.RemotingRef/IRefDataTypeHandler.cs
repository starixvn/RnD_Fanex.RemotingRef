namespace Fanex.RemotingRef
{
    using TranslationStorage;

    internal interface IRefDataTypeHandler
    {
        RefDataTypeHandlerOptions Options { get; }

        object Get(KeySet keySet, string languageCode);
    }
}