namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using PublicEnum.Language;

    public interface ISportService
    {
        string GetSportById(string sportId, string languageCode);
    }

    public class SportService : ISportService
    {
        public SportService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.Sport";

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