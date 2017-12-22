namespace Fanex.RemotingRef
{
    using TranslationStorage;

    public interface IRefDataTypeHandler
    {
        RefDataTypeHandlerOptions Options { get; }

        object Get(KeySet keySet, string languageCode);
    }
}