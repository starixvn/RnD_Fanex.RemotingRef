using RefClient.Service.IService;
using RefDataModelStandard.Models.Request;
using RefClient.Factory;
using RefClient.Service;
using PublicEnum.Language;
using RefDataModelStandard.Enums;

namespace Fanex.RemotingRef.NetCore.Utilities.RefData
{
    public class RefDataHelper : IRefDataHelper
    {
        private const string DefaultValue = null;

        private IBetRefClientService _betRefClient = RefClientFactory.GetBetRefClientService();
        private IRefDataClient _refDataClient = RefClientFactory.GetRefDataClient();
        private IThirdPartyRefClientService _thirdPartyRefClientService = RefClientFactory.GetThirdPartyRefClientService();

        //public RefDataHelper(IBetRefClientService betRefClientService)
        //=> (_betRefClient) = (betRefClientService);

        public string GetBetTypeName(
            string betTypeId,
            string resourceId,
            long? refNo,
            string language)
        {
            var languageEnum = language.ConvertCultureToEnum();
            if (int.TryParse(betTypeId, out int id))
            {
                var bettypeName = _betRefClient.GetBetTypeName(GetBetTypeNameModel.CreateModel(id, languageEnum, resourceId));
                var luckyVia = refNo.HasValue && refNo > 100000000000 ?
                    _betRefClient.GetComboParlayLuckyBet(new GetComboParlayLuckyBetModel { Language = languageEnum }) :
                    string.Empty;

                return string.IsNullOrEmpty(luckyVia) ? bettypeName : $"{bettypeName} {luckyVia}";
            }

            return DefaultValue;
        }

        public string GetSportName(string sportTypeId, string language)
        => int.TryParse(sportTypeId, out int id) ?
                _refDataClient.GetSportName(new GetSportNameModel
                {
                    SportType = id,
                    Language = language.ConvertCultureToEnum()
                })
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult()
                : DefaultValue;

        public string GetTeamName(string teamId, string language)
        => int.TryParse(teamId, out int id) ?
             _refDataClient.GetTeamName(new GetTeamNameModel
             {
                 TeamId = id,
                 Language = language.ConvertCultureToEnum()
             })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
            : DefaultValue;

        public string GetTeamName3rd(string team3rdId, string language)
        => int.TryParse(team3rdId, out int id) ?
             _refDataClient.GetThirdPartyTeamName(new GetThirdPartyTeamNameModel
             {
                 ThirdPartyTeamId = id,
                 Language = language.ConvertCultureToEnum()
             })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
            : DefaultValue;

        public string GetLeagueName(string leagueId, string language)
        => int.TryParse(leagueId, out int id) ?
            _refDataClient.GetLeagueName(new GetLeagueNameModel
            {
                LeagueId = id,
                Language = language.ConvertCultureToEnum()
            })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
            : DefaultValue;


        public string GetLeagueName3rd(string league3rdId, string language)
        => int.TryParse(league3rdId, out int id) ?
             _thirdPartyRefClientService.GetThirdPartyLeagueName(new GetThirdPartyLeagueNameModel
             {
                 ThirdPartyLeagueId = id,
                 Language = language.ConvertCultureToEnum()
             })
            : DefaultValue;


        public string GetCountryName(string countryCode, string language)
          => _refDataClient.GetCountryName(new GetCountryNameModel
          {
              CountryCode = countryCode,
              Language = language.ConvertCultureToEnum()
          })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        public string GetOddsTypeName(string oddsTypeId, string language)
        => _betRefClient.GetOddsTypeName(new GetOddsTypeNameModel
        {
            OddsTypeId = oddsTypeId,
            Language = language.ConvertCultureToEnum()
        });

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string language)
        {
            var data = _refDataClient.GetResourceDictionary(new GetResourceDictionaryModel { RefId = refId })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
            if (data != null && data.ContainsKey(resourceId))
            {
                var resource = data[resourceId];

                return resource.GetName(language.ConvertCultureToEnum());
            }
            return string.Empty;
        }

        public string GetParentIdByBetTypeId(string betTypeId)
        => int.TryParse(betTypeId, out int id) ?
        _betRefClient.GetParentIdByBetTypeId(new GetParentIdByBetTypeIdModel { BetTypeId = id }).ToString()
        : DefaultValue;

        public string[] GetCombinationTypeCombo(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            string language)
        {
            var list = new List<string>();
            if (Enum.TryParse(betCheck, out BetCheckEnum id))
            {
                list = _betRefClient.GetCombinationTypeCombo(new GetCombinationTypeComboModel
                {
                    BetCheck = id,
                    BetTeam = betTeam,
                    Hdp1 = handicap1,
                    Hdp2 = handicap2,
                    Language = language.ConvertCultureToEnum()
                }).ToList();
            }

            return list.ToArray();
        }

        public string GetHorseTeamName(string codeId, string language)
        => codeId;

        public string GetCasinoBetItem(string itemId, string language)
        => _betRefClient.GetCasinoBetItem(new GetCasinoBetItemModel { ItemId = itemId, Language = language.ConvertCultureToEnum() });

        public string GetRouletteBetItem(string itemId, string language)
        => _betRefClient.GetRouletteBetItem(new GetRouletteBetItemModel { ItemId = itemId, Language = language.ConvertCultureToEnum() });


        public string GetKenoBetChoice(string kenoBetChoiceId, string language)
        => _betRefClient.GetKenoBetChoice(new GetKenoBetChoiceModel { KenoBetChoiceId = kenoBetChoiceId }).GetName(language.ConvertCultureToEnum());


        public string GetKenoBetType(string kenoBetTypeId, string language)
        => _betRefClient.GetKenoBetType(new GetKenoBetTypeModel { KenoBetTypeId = kenoBetTypeId, Language = language.ConvertCultureToEnum() });

        public string GetFastMarketsChoice(
            string betTypeId,
            string betTeam,
            int handicap1,
            int handicap2,
            string language)
        => int.TryParse(betTeam, out int betTeamId)
        && int.TryParse(betTypeId, out int id)
        && Enum.TryParse(handicap2.ToString(), out FastMarketBetOptionsEnum enumid) ?
            _refDataClient.GetFastMarketChoice(new GetFastMarketsChoiceModel
            {
                BetTeam = betTeamId,
                BetType = id,
                Hour = handicap1,
                FastMarketBetOptionsEnum = enumid,
                Language = language.ConvertCultureToEnum()
            })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
            : DefaultValue;


        public string GetFastMarketsResult(string resultId, string language)
        {
            var list = new List<string>();
            if (Enum.TryParse(resultId, out FastMarketBetOptionsEnum id))
            {
                list = _refDataClient.GetFastMarketBetOptionName(new GetFastMarketsResultModel
                {
                    Language = language.ConvertCultureToEnum(),
                    FastMarketBetOptionsEnum = id
                })
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }

            return list.Any() ? list.FirstOrDefault() : resultId;
        }

        public string GetFastMarkets225Result(string resultId, string language)
        {
            var list = new List<string>();
            if (Enum.TryParse(resultId, out FastMarketBetOptionsEnum id))
            {
                list = _refDataClient.GetFastMarketBetOptionNameOfBetType225(new GetFastMarketsResultModel
                {
                    Language = language.ConvertCultureToEnum(),
                    FastMarketBetOptionsEnum = id
                })
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }

            return list.Any() ? list.FirstOrDefault() : resultId;
        }

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string language)
                => _refDataClient.GetLotteryBetChoice(new GetLotteryBetChoiceModel { LotteryBetChoiceId = lotteryBetChoiceId })
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
                .GetName(language.ConvertCultureToEnum());

        public string GetLotteryBetType(string lotteryBetTypeId, string language)
        => _betRefClient.GetLotteryBetType(new GetLotteryBetTypeModel
        {
            LotteryBetTypeId = lotteryBetTypeId,
            Language = language.ConvertCultureToEnum()
        });

        public string GetBetRadarMatchCodeFormat(string matchCode, string language)
        => _refDataClient.GetBetradarMatchCodeFormat(new GetBetradarMatchCodeFormatModel
        {
            MatchCode = matchCode,
            Language = language.ConvertCultureToEnum()
        })
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();

        public string GetDisplayedTypeId(string betTypeId)
        => int.TryParse(betTypeId, out int id) ?
        _betRefClient.GetBetTypeOfDisplayTypeId(new GetBetTypeOfDisplayTypeIdModel { BetType = id }).ToString()
        : DefaultValue;

        public string GetBetBuilderSelectionName(int selectionId, string language)
        {
            var selectionNames = _refDataClient.GetBetBuilderSelectionName(new GetBetBuilderSelectionNameModel
            {
                SelectionIdList = new List<int> { selectionId },
                Language = language.ConvertCultureToEnum()
            })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

            return selectionNames.ContainsKey(selectionId) ? selectionNames[selectionId] : DefaultValue;
        }

        public string GetTableGamesBetChoice(string tableGamesBetChoiceId, string language)
        => _refDataClient.GetTableGameBetChoice(new GetTableGameBetChoiceModel
        {
            ChoiceId = tableGamesBetChoiceId,
            Language = language.ConvertCultureToEnum()
        })
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult()
        .GetName(language.ConvertCultureToEnum());

        public string GetTableGamesBetType(string tableGamesBetTypeId, string language)
        => _refDataClient.GetTableGameBetType(new GetTableGameBetTypeModel
        {
            BetType = tableGamesBetTypeId,
            Language = language.ConvertCultureToEnum()
        })
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();
    }
}