namespace Fanex.RemotingRef.NetCore.Services
{
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef.NetCore.Criteria;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using Fanex.RemotingRef.NetCore.Utilities;
    using PublicEnum.Language;
    using RefDataModelStandard.Utils;



    public class BettypeService(IDynamicRepository repository, ICacheService cacheService, IRefResourceService refResourceService) : IBettypeService
    {
        private readonly List<int> specialBetTypeForBaseball = [8810, 8811, 8812, 8813, 8823, 8824];
        private const string cachedKey = "Fanex.RemotingRef.NetCode.Bettype";

        private readonly IDynamicRepository _dynamicRepository = repository;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IRefResourceService _refResourceService = refResourceService;

        public string GetBettypeById(string bettypeId, string languageCode)
        {
            if (int.TryParse(bettypeId, out int id))
            {
                var bettypes = _cacheService.GetOrSet(cachedKey, () => _dynamicRepository.Fetch<BetTypeModel>(new GetBettypeCriteria()));
                var bettype = bettypes.SingleOrDefault(t => t.TypeId == id);

                return bettype?.GetName(languageCode.ConvertCultureToEnum()) ?? string.Empty;
            }

            return string.Empty;
        }

        public string GetBettypeById(string bettypeId, string resourceId, long? refNo, string languageCode)
        {
            if (int.TryParse(bettypeId, out int id))
            {
                var betTypeName = string.Empty;
                var luckyVia = refNo.HasValue && refNo > 100_000_000_000 ?
                            _refResourceService.GetRefResourceById("ComboParlay", "LuckyBet", languageCode) :
                            string.Empty;

                betTypeName = specialBetTypeForBaseball.Exists(i => i == id) && languageCode == LanguageHelpers.DefaultLanguage
                    ? GetBetTypeNameForBaseball(bettypeId, languageCode, resourceId)
                    : GetBetTypeNameWithReplaceText(id, resourceId, languageCode);

                return string.IsNullOrEmpty(luckyVia) ? betTypeName : $"{betTypeName} {luckyVia}";
            }

            return string.Empty;
        }

        private string GetBetTypeNameForBaseball(string id, string language, string resourceId)
        {
            var betTypeName = GetBettypeById(id, language);
            var replacedText = new List<string> { "X", "First", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th" };
            var isNumber = int.TryParse(resourceId, out int resId);
            if (isNumber)
            {
                resourceId = resId >= replacedText.Count ? resourceId : replacedText[resId];
            }

            return string.IsNullOrEmpty(resourceId) ? betTypeName : betTypeName.Replace("X ", $"{resourceId} ");
        }

        private string GetBetTypeNameWithReplaceText(int id, string resourceId, string language)
        {
            var betTypeName = GetBettypeById(id.ToString(), language);
            var replaceSymbolModel = new ResourceIdParser().Parse(id, resourceId);

            if (replaceSymbolModel == null)
            {
                return betTypeName;
            }

            static void ReplacePlaceholder(ref string name, string placeholder, int? value)
            {
                if (value.HasValue)
                {
                    string replacement = value.Value.ToString();
                    name = name.Replace($" {placeholder} ", $" {replacement} ")
                    .Replace($" {placeholder}", $" {replacement}")
                    .Replace($"{placeholder} ", $"{replacement} ")
                    .Replace($"{placeholder}°", $"{replacement}°")
                    .Replace($"{placeholder}η", $"{replacement}η");
                }
            }

            ReplacePlaceholder(ref betTypeName, "X", replaceSymbolModel.X);
            ReplacePlaceholder(ref betTypeName, "x", replaceSymbolModel.X);
            ReplacePlaceholder(ref betTypeName, "Y", replaceSymbolModel.Y);
            ReplacePlaceholder(ref betTypeName, "y", replaceSymbolModel.Y);
            ReplacePlaceholder(ref betTypeName, "Z", replaceSymbolModel.Z);
            ReplacePlaceholder(ref betTypeName, "z", replaceSymbolModel.Z);

            return betTypeName;
        }
    }
}