namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using PublicEnum.Language;
    public interface ILeague3rdService
    {
        string GetLeague3rdById(string league3rdId, string languageCode);

        Dictionary<int, League3rdModel> GetAllLeagues();
    }

    public class League3rdService : ILeague3rdService
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.League3rd";

        public League3rdService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        public string GetLeague3rdById(string league3rdId, string languageCode)
        {
            if (int.TryParse(league3rdId, out var id))
            {
                var leagues = _cacheService.Get<Dictionary<int, League3rdModel>>(CachedKey);
                if (leagues == null)
                {
                    leagues = GetAllLeagues();
                }

                if (!leagues.TryGetValue(id, out var league))
                {
                    leagues = GetAllLeagues();
                    if (!leagues.TryGetValue(id, out league))
                    {
                        return string.Empty;
                    }
                }

                var name = league.GetName(languageCode.ConvertCultureToEnum());

                return name;
            }

            return string.Empty;
        }

        public Dictionary<int, League3rdModel> GetAllLeagues()
        {
            var leagues = _dynamicRepository.Fetch<League3rdModel>(new GetLeague3rdCriteria()).ToDictionary(i => i.League3rdId);
            _cacheService.Set<Dictionary<int, League3rdModel>>(CachedKey, leagues, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return leagues;
        }
    }
}