namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using PublicEnum.Language;

    public class SportService(IDynamicRepository repository, ICacheService cacheService) : ISportService
    {
        private const string CachedKey = "Fanex.RemotingRef.NetCode.Sport";
        private readonly IDynamicRepository _dynamicRepository = repository;
        private readonly ICacheService _cacheService = cacheService;

        public string GetSportById(string sportId, string languageCode)
        {
            if (int.TryParse(sportId, out var id))
            {
                var sports = _cacheService.GetOrSet(CachedKey, () => _dynamicRepository.Fetch<SportModel>(new GetSportCriteria()));
                var sport = sports.SingleOrDefault(t => t.SportType == id);
                return sport?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
            }

            return string.Empty;
        }
    }
}