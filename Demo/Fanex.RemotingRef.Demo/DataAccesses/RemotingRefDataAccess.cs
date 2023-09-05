namespace Fanex.RemotingRef.Demo.DataAccesses
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;

    public class RemotingRefDataAccess : IRemotingRefDataAccess
    {
        private const string DefaultValue = "No Data";
        private readonly IRemotingRefService _remotingRefService;

        public RemotingRefDataAccess(IRemotingRefService remotingRefService)
        {
            _remotingRefService = remotingRefService;
        }

        public RefDataResult GetBetTypeName(string betTypeId, long refNo = 0, string resourceId = Common.DefaultResourceId, string languageCode = Common.DefaultLanguage)
        {
            return new RefDataResult
            {
                FanexRefNew = GetBetTypeNameFanexRefNew(betTypeId, refNo, resourceId, languageCode),
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

        public RefDataResult GetSportName(string sportTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetSportName(sportTypeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetLeagueName(string leagueId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLeagueName(leagueId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetLeagueName3rd(string leagueId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLeagueName3rd(leagueId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetTeamName(string teamId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTeamName(teamId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetTeamName3rd(string teamId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTeamName3rd(teamId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetResourceData(string refId, string resourceId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetCountryName(string countryId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetCountryName(countryId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetOddsTypeName(string oddsTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetOddsTypeName(oddsTypeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetParentIdByBetTypeId(string betTypeId)
        {
            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetParentIdByBetTypeId(betTypeId, DefaultValue),
            };
        }

        public RefDataResult GetHorseTeamName(string codeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetHorseTeamName(codeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetCasinoBetItem(string itemId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetCasinoBetItem(itemId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetRouletteBetItem(string itemId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetRouletteBetItem(itemId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetKenoBetType(string kenoBetTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetKenoBetType(kenoBetTypeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetTableGamesBetType(string tableGamesBetTypeId, string languageCode = "en-US")
        {
            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTableGamesBetType(tableGamesBetTypeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetTableGamesBetChoice(string tableGamesBetChoiceId, string languageCode = "en-US")
        {
            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTableGamesBetChoice(tableGamesBetChoiceId, languageCode,out string DefaultValue),
            };
        }

        public RefDataResult GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetFastMarketsChoice(betTypeId, betTeam, handicap1, handicap2, twLanguage),
            };
        }

        public RefDataResult GetFastMarketsResult(string resultId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetFastMarketsResult(resultId, languageCode),
            };
        }

        public RefDataResult GetFastMarkets225Result(string resultId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetFastMarkets225Result(resultId, languageCode),
            };
        }

        public RefDataResult GetLotteryBetType(string betTypeId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLotteryBetType(betTypeId, languageCode, DefaultValue),
            };
        }

        public RefDataResult GetBetRadarMatchCodeFormat(string matchCode, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetBetRadarMatchCodeFormat(matchCode, languageCode),
            };
        }

        public RefDataResult GetDisplayedTypeId(string betTypeId)
            => new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetDisplayedTypeId(betTypeId),
            };
    }
}