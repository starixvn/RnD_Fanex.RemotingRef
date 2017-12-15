namespace Fanex.RemotingRef
{
    using TranslationStorage;

    public interface IDataTypeHandler
    {
        DataTypeHandlerOptions Options { get; }

        string Get(ExtraDataSet parameters, string languageCode, string defaultValue);
    }
}