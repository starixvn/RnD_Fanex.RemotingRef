namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.RemotingRef.NetCore.Models;

    public interface ILeague3rdService
    {
        string GetLeague3rdById(string league3rdId, string languageCode);

        Dictionary<int, League3rdModel> GetAllLeagues();
    }
}