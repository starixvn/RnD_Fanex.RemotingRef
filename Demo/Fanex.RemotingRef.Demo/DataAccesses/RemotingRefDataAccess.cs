namespace Fanex.RemotingRef.Demo.DataAccesses
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using global::RemotingRef;

    public class RemotingRefDataAccess : IRemotingRefDataAccess
    {
        private const string DefaultValue = "No Data";
        private readonly IRemotingRefService _remotingRefService;
        private readonly IRefData _taiwanRefNew;

        public RemotingRefDataAccess(IRemotingRefService remotingRefService, IRefData taiwanRefNew)
        {
            _remotingRefService = remotingRefService;
            _taiwanRefNew = taiwanRefNew;
        }

        public RefDataResult GetBetTypeName(string betTypeId, long refNo = 0, string resourceId = Common.DefaultResourceId, string languageCode = Common.DefaultLanguage)
        {
            return new RefDataResult
            {
                FanexRefNew = GetBetTypeNameFanexRefNew(betTypeId, refNo, resourceId, languageCode),
                TaiwanRefOld = GetBetTypeNameTWRefOld(betTypeId, refNo, resourceId, languageCode),
                TaiwanRefNew = GetBetTypeNameTWRefNew(betTypeId, refNo, resourceId, languageCode)
            };
        }

        private string GetBetTypeNameFanexRefNew(string typeId, long refNo, string resourceId, string languageCode)
        {
            var betTypeName = string.Empty;

            if (refNo == 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = _remotingRefService.GetBetTypeName(typeId, languageCode);
            }
            else if (refNo != 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = _remotingRefService.GetBetTypeName(typeId, languageCode, refNo);
            }
            else if (refNo == 0 && resourceId != Common.DefaultResourceId)
            {
                betTypeName = _remotingRefService.GetBetTypeName(typeId, languageCode, DefaultValue, resourceId);
            }
            else
            {
                betTypeName = _remotingRefService.GetBetTypeName(typeId, languageCode, DefaultValue, resourceId, refNo);
            }

            return betTypeName;
        }

        private static string GetBetTypeNameTWRefOld(string typeId, long refNo, string resourceId, string languageCode)
        {
            var betTypeName = string.Empty;
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            if (refNo == 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = RefData.GetBetTypeName(typeId, twLanguage);
            }
            else if (refNo != 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = RefData.GetBetTypeName(typeId, twLanguage, refNo);
            }
            else if (refNo == 0 && resourceId != Common.DefaultResourceId)
            {
                betTypeName = RefData.GetBetTypeName(typeId, twLanguage, DefaultValue, resourceId);
            }
            else
            {
                betTypeName = RefData.GetBetTypeName(typeId, twLanguage, DefaultValue, resourceId, refNo);
            }

            return betTypeName;
        }

        private string GetBetTypeNameTWRefNew(string typeId, long refNo, string resourceId, string languageCode)
        {
            var betTypeName = string.Empty;
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            if (refNo == 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = _taiwanRefNew.GetBetTypeName(typeId, twLanguage, DefaultValue);
            }
            else if (refNo != 0 && resourceId == Common.DefaultResourceId)
            {
                betTypeName = _taiwanRefNew.GetBetTypeName(typeId, twLanguage, refNo, DefaultValue);
            }
            else if (refNo == 0 && resourceId != Common.DefaultResourceId)
            {
                betTypeName = _taiwanRefNew.GetBetTypeName(typeId, twLanguage, DefaultValue, resourceId);
            }
            else
            {
                betTypeName = _taiwanRefNew.GetBetTypeName(typeId, twLanguage, DefaultValue, resourceId, refNo);
            }

            return betTypeName;
        }

        public RefDataResult GetSportName(string sportTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetSportName(sportTypeId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetSportName(sportTypeId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetSportName(sportTypeId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetLeagueName(string leagueId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLeagueName(leagueId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetLeagueName(leagueId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetLeagueName(leagueId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetTeamName(string teamId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTeamName(teamId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetTeamName(teamId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetTeamName(teamId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetResourceData(string refId, string resourceId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetResourceDataByRefIdAndResourceId(refId, resourceId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetResourceDataByRefIdAndResourceId(refId, resourceId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetCountryName(string countryId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetCountryName(countryId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetCountryName(countryId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetCountryName(countryId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetOddsTypeName(string oddsTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetOddsTypeName(oddsTypeId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetOddsTypeName(oddsTypeId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetOddsTypeName(oddsTypeId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetParentIdByBetTypeId(string betTypeId, string languageCode = "en-US")
        {
            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue),
                TaiwanRefOld = RefData.GetParentIdByBetTypeId(betTypeId, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetParentIdByBetTypeId(betTypeId, DefaultValue)
            };
        }

        public RefDataResult GetHorseTeamName(string codeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetHorseTeamName(codeId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetHorseTeamName(codeId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetHorseTeamName(codeId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetCasinoBetItem(string itemId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetCasinoBetItem(itemId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetCasinoBetItem(itemId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetCasinoBetItem(itemId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetRouletteBetItem(string itemId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetRouletteBetItem(itemId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetRouletteBetItem(itemId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetRouletteBetItem(itemId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetKenoBetType(string kenoBetTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetKenoBetType(kenoBetTypeId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetKenoBettype(kenoBetTypeId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetKenoBettype(kenoBetTypeId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, twLanguage),
                TaiwanRefOld = RefData.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, twLanguage),
                TaiwanRefNew = _taiwanRefNew.GetFastMarketsChoise(betTypeId, betTeam, handicap1, handicap2, twLanguage)
            };
        }

        public RefDataResult GetFastMarkets225Result(string resultId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetFastMarkets225Result(resultId, languageCode),
                TaiwanRefOld = RefData.GetFastMarkets225Result(resultId, twLanguage),
                TaiwanRefNew = _taiwanRefNew.GetFastMarkets225Result(resultId, twLanguage)
            };
        }

        public RefDataResult GetLotteryBetType(string betTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLotteryBetType(betTypeId, languageCode, DefaultValue),
                TaiwanRefOld = RefData.GetLotteryBettype(betTypeId, twLanguage, DefaultValue),
                TaiwanRefNew = _taiwanRefNew.GetLotteryBettype(betTypeId, twLanguage, DefaultValue)
            };
        }
    }
}