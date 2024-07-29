using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.RemotingRef.NetCore.Criteria;
using Fanex.RemotingRef.NetCore.Models;
using PublicEnum.Language;

namespace Fanex.RemotingRef.NetCore.Services
{
    public interface ILeague3rdService
    {
        string GetLeague3rdById(string league3rdId, string languageCode);

        IEnumerable<League3rdModel> GetLeagues();
    }

    public class League3rdService : ILeague3rdService
    {
        public League3rdService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.League3rd";
        public string GetLeague3rdById(string league3rdId, string languageCode)
        {
            if (int.TryParse(league3rdId, out var id))
            {
                var leagues = _cacheService.Get<IEnumerable<League3rdModel>>(CachedKey);
                if (leagues == null)
                {
                    leagues = GetLeagues();
                }

                var league = leagues.SingleOrDefault(t => t.League3rdId == id) ?? GetLeagues().SingleOrDefault(t => t.League3rdId == id);
                return league?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
            }

            return string.Empty;
        }

        public IEnumerable<League3rdModel> GetLeagues()
        {
            var timeSpan = _cacheService.Get<long>($"{CachedKey}.TimeSpan");
            var teams = _dynamicRepository.Fetch<League3rdModel>(new GetLeague3rdCriteria(timeSpan));
            _cacheService.Set<IEnumerable<League3rdModel>>(CachedKey, teams);
            if (teams != null)
            {
                var last = teams.LastOrDefault();
                _cacheService.Set<long>($"{CachedKey}.TimeSpan", last != null ? last.TimeSpan : 0);
            }

            return teams;
        }
    }
}
