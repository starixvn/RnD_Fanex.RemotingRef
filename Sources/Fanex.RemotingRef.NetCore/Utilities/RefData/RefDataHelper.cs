namespace Fanex.RemotingRef.NetCore.Utilities.RefData
{
    using RefClient.Service.IService;
    using RefDataModelStandard.Models.Request;
    using RefClient.Factory;
    using RefClient.Service;
    using PublicEnum.Language;
    using RefDataModelStandard.Enums;
    using System.Linq;

    public class RefDataHelper : IRefDataHelper
    {
        private const string DefaultValue = null;

        private IBetRefClientService _betRefClient = RefClientFactory.GetBetRefClientService();
        private IRefDataClient _refDataClient = RefClientFactory.GetRefDataClient();
        private IThirdPartyRefClientService _thirdPartyRefClientService = RefClientFactory.GetThirdPartyRefClientService();

        public string GetBetTypeName(
            string betTypeId,
            string resourceId,
            long? refNo,
            string language)
        {
            var languageEnum = language.ConvertCultureToEnum();
            var bettypeName = _betRefClient.GetBetTypeName(GetBetTypeNameModel.CreateModel(int.Parse(betTypeId), languageEnum, resourceId));
            var luckyVia = refNo.HasValue && refNo > 100000000000 ?
                _betRefClient.GetComboParlayLuckyBet(new GetComboParlayLuckyBetModel { Language = languageEnum }) :
                string.Empty;

            return string.IsNullOrEmpty(luckyVia) ? bettypeName : $"{bettypeName} {luckyVia}";
        }

        public string GetSportName(string sportTypeId, string language)
        => _refDataClient.GetSportName(new GetSportNameModel
        {
            SportType = int.Parse(sportTypeId),
            Language = language.ConvertCultureToEnum()
        })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        public string GetTeamName(string teamId, string language)
        => _refDataClient.GetTeamName(new GetTeamNameModel
        {
            TeamId = int.Parse(teamId),
            Language = language.ConvertCultureToEnum()
        })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        public string GetTeamName3rd(string team3rdId, string language)
        => _refDataClient.GetThirdPartyTeamName(new GetThirdPartyTeamNameModel
        {
            ThirdPartyTeamId = int.Parse(team3rdId),
            Language = language.ConvertCultureToEnum()
        })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        public string GetLeagueName(string leagueId, string language)
        => _refDataClient.GetLeagueName(new GetLeagueNameModel
        {
            LeagueId = int.Parse(leagueId),
            Language = language.ConvertCultureToEnum()
        })
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();

        public string GetLeagueName3rd(string league3rdId, string language)
        => _thirdPartyRefClientService.GetThirdPartyLeagueName(new GetThirdPartyLeagueNameModel
        {
            ThirdPartyLeagueId = int.Parse(league3rdId),
            Language = language.ConvertCultureToEnum()
        });

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
        => _betRefClient.GetParentIdByBetTypeId(new GetParentIdByBetTypeIdModel { BetTypeId = int.Parse(betTypeId) }).ToString();

        public string[] GetCombinationTypeCombo(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            string language)
        {
            var list = _betRefClient.GetCombinationTypeCombo(new GetCombinationTypeComboModel
            {
                BetCheck = (BetCheckEnum)Enum.Parse(typeof(BetCheckEnum), betCheck),
                BetTeam = betTeam,
                Hdp1 = handicap1,
                Hdp2 = handicap2,
                Language = language.ConvertCultureToEnum()
            });

            return list != null ? list.ToArray() : new string[] { };
        }
        public string GetHorseTeamName(string codeId, string language)
        => codeId;
        //  => RefData.GetHorseTeamName(codeId, language, DefaultValue);

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
            => _refDataClient.GetFastMarketChoice(new GetFastMarketsChoiceModel
            {
                BetTeam = int.Parse(betTeam),
                BetType = int.Parse(betTypeId),
                Hour = handicap1,
                FastMarketBetOptionsEnum = (FastMarketBetOptionsEnum)handicap2,
                Language = language.ConvertCultureToEnum()
            })
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        public string GetFastMarketsResult(string resultId, string language)
        {
            var list = _refDataClient.GetFastMarketBetOptionName(new GetFastMarketsResultModel
            {
                Language = language.ConvertCultureToEnum(),
                FastMarketBetOptionsEnum = (FastMarketBetOptionsEnum)int.Parse(resultId)
            })
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

            return list.Any() ? list.FirstOrDefault() : resultId;
        }

        public string GetFastMarkets225Result(string resultId, string language)
        {
            var list = _refDataClient.GetFastMarketBetOptionNameOfBetType225(new GetFastMarketsResultModel
            {
                Language = language.ConvertCultureToEnum(),
                FastMarketBetOptionsEnum = (FastMarketBetOptionsEnum)int.Parse(resultId)
            })
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

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
        => _refDataClient.GetBetradarMatchCodeFormat(new GetBetradarMatchCodeFormatModel { MatchCode = matchCode, Language = language.ConvertCultureToEnum() })
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();

        public string GetDisplayedTypeId(string betTypeId)
        => _betRefClient.GetBetTypeOfDisplayTypeId(new GetBetTypeOfDisplayTypeIdModel { BetType = int.Parse(betTypeId) }).ToString();

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

            return selectionNames.ContainsKey(selectionId) ? selectionNames[selectionId] : default;
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