namespace Fanex.RemotingRef
{
    using TranslationStorage;

    public interface IDataTypeHandler
    {
        DataTypeHandlerOptions Options { get; }

        string Get(KeySet keySet, string languageCode);
    }
}