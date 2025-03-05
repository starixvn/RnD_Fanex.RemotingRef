namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using PublicEnum.Language;

    public interface ITeam3rdService
    {
        string GetTeam3rdById(string team3rdId, string languageCode);

        Dictionary<int, Team3rdModel> GetAllTeams();
    }

    public class Team3rdService : ITeam3rdService
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.Team3rd";

        public Team3rdService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        public string GetTeam3rdById(string team3rdId, string languageCode)
        {
            if (int.TryParse(team3rdId, out var id))
            {
                var teams = _cacheService.Get<Dictionary<int, Team3rdModel>>(CachedKey);
                if (teams == null)
                {
                    teams = GetAllTeams();
                }

                if (!teams.TryGetValue(id, out var team))
                {
                    teams = GetAllTeams();
                    if (!teams.TryGetValue(id, out team))
                    {
                        return string.Empty;
                    }
                }

                var name = team.GetName(languageCode.ConvertCultureToEnum());

                return name;
            }

            return string.Empty;
        }

        public Dictionary<int, Team3rdModel> GetAllTeams()
        {
            var teams = _dynamicRepository.Fetch<Team3rdModel>(new GetTeam3rdCriteria()).ToDictionary(i => i.Team3rdId);
            _cacheService.Set<Dictionary<int, Team3rdModel>>(CachedKey, teams, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return teams;
        }
    }
}