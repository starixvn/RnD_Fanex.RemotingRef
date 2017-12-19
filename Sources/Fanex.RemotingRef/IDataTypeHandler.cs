namespace Fanex.RemotingRef
{
    using TranslationStorage;

    public interface IDataTypeHandler
    {
        DataTypeHandlerOptions Options { get; }

        object Get(KeySet keySet, string languageCode);
    }
}