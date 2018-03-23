namespace Fanex.RemotingRef.Demo.DataAccesses
{
    using Fanex.RemotingRef.Demo.Constants;
    using Fanex.RemotingRef.Demo.Models;
    using global::RemotingRef;

    public class RemotingRefDataAccess : IRemotingRefDataAccess
    {
        private const string DefaultValue = "exception has been thrown";
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
                FanexRefNew = _remotingRefService.GetSportName(sportTypeId, languageCode),
                TaiwanRefOld = RefData.GetSportName(sportTypeId, twLanguage),
                TaiwanRefNew = _taiwanRefNew.GetSportName(sportTypeId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetLeagueName(string leagueId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetLeagueName(leagueId, languageCode),
                TaiwanRefOld = RefData.GetLeagueName(leagueId, twLanguage),
                TaiwanRefNew = _taiwanRefNew.GetLeagueName(leagueId, twLanguage, DefaultValue)
            };
        }

        public RefDataResult GetTeamName(string teamId, string languageCode = "en-US")
        {
            var twLanguage = Common.GenerateTaiwanLanguage(languageCode);

            return new RefDataResult
            {
                FanexRefNew = _remotingRefService.GetTeamName(teamId, languageCode),
                TaiwanRefOld = RefData.GetTeamName(teamId, twLanguage),
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
                TaiwanRefOld = RefData.GetCountryName(countryId, twLanguage),
                TaiwanRefNew = _taiwanRefNew.GetCountryName(countryId, twLanguage, DefaultValue)
            };
        }
    }
}