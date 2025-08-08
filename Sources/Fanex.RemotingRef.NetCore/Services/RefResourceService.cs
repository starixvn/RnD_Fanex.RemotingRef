namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using PublicEnum.Language;

    public interface IRefResourceService
    {
        string GetRefResourceById(string refId, string resourceId, string languageCode);
    }

    public class RefResourceService : IRefResourceService
    {
        public RefResourceService(IDynamicRepository repository, ICacheService cacheService)
        {
            _dynamicRepository = repository;
            _cacheService = cacheService;
        }

        private readonly IDynamicRepository _dynamicRepository;
        private readonly ICacheService _cacheService;
        private const string CachedKey = "Fanex.RemotingRef.NetCode.RefResource";

        public string GetRefResourceById(string refId, string resourceId, string languageCode)
        {
            var items = _cacheService.GetOrSet(CachedKey, () => _dynamicRepository.Fetch<RefResourceModel>(new GetRefResourceCriteria()));
            var item = items.SingleOrDefault(t => t.RefId == refId && t.ResourceId == resourceId);

            return item?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
        }
    }
}