namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using PublicEnum.Language;

    public class League3rdService(IDynamicRepository repository, ICacheService cacheService) : ILeague3rdService
    {
        private const string CachedKey = "Fanex.RemotingRef.NetCode.League3rd";
        private readonly IDynamicRepository _dynamicRepository = repository;
        private readonly ICacheService _cacheService = cacheService;

        public string GetLeague3rdById(string league3rdId, string languageCode)
        {
            if (int.TryParse(league3rdId, out var id))
            {
                var leagues = _cacheService.Get<Dictionary<int, League3rdModel>>(CachedKey);
                leagues ??= GetAllLeagues();

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