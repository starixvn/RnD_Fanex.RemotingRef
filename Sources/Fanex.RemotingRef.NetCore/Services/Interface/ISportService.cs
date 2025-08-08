namespace Fanex.RemotingRef.NetCore.Services.Interface
{
    public interface ISportService
    {
        string GetSportById(string sportId, string languageCode);
    }
}