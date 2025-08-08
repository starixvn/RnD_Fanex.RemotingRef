namespace Fanex.RemotingRef.NetCore.Services.Interface
{
    public interface IBettypeService
    {
        string GetBettypeById(string bettypeId, string languageCode);

        string GetBettypeById(string bettypeId, string resourceId, long? refNo, string languageCode);
    }
}