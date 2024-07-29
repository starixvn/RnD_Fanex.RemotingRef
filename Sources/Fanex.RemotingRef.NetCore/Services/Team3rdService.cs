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

        IEnumerable<Team3rdModel> GetTeams();
    }

    public class Team3rdService : ITeam3rdService
    {
        public Team3rdService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.Team3rd";

        public string GetTeam3rdById(string team3rdId, string languageCode)
        {
            if (int.TryParse(team3rdId, out var id))
            {
                var teams = _cacheService.Get<IEnumerable<Team3rdModel>>(CachedKey);
                if (teams == null)
                {
                    teams = GetTeams();
                }
                var team = teams.SingleOrDefault(t => t.Team3rdId == id) ?? GetTeams().SingleOrDefault(t => t.Team3rdId == id);
                return team?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
            }

            return string.Empty;
        }

        public IEnumerable<Team3rdModel> GetTeams()
        {
            var timeSpan = _cacheService.Get<long>($"{CachedKey}.TimeSpan");
            var teams = _dynamicRepository.Fetch<Team3rdModel>(new GetTeam3rdCriteria(timeSpan));
            _cacheService.Set<IEnumerable<Team3rdModel>>(CachedKey, teams);
            if (teams != null)
            {
                var last = teams.LastOrDefault();
                _cacheService.Set<long>($"{CachedKey}.TimeSpan", last != null ? last.TimeSpan : 0);
            }

            return teams;
        }
    }
}