namespace Fanex.RemotingRef.NetCore.Services.Interface
{
    using Fanex.RemotingRef.NetCore.Models;

    public interface ITeam3rdService
    {
        string GetTeam3rdById(string team3rdId, string languageCode);

        Dictionary<int, Team3rdModel> GetAllTeams();
    }
}