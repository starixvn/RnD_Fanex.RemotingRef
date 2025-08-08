namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using PublicEnum.Language;

    public class RefResourceService(IDynamicRepository repository, ICacheService cacheService) : IRefResourceService
    {
        private const string CachedKey = "Fanex.RemotingRef.NetCode.RefResource";
        private readonly IDynamicRepository _dynamicRepository = repository;
        private readonly ICacheService _cacheService = cacheService;

        public string GetRefResourceById(string refId, string resourceId, string languageCode)
        {
            var items = _cacheService.GetOrSet(CachedKey, () => _dynamicRepository.Fetch<RefResourceModel>(new GetRefResourceCriteria()));
            var item = items.SingleOrDefault(t => t.RefId == refId && t.ResourceId == resourceId);

            return item?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
        }
    }
}