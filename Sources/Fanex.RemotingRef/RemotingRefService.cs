namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TranslationStorage;

    public class RemotingRefService : IRemotingRefService
    {
        private const string RefController = "ref/";
        private const string RefSyncController = "refsync/";

        public string GetBetTypeName(string betTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };

            return GetBetTypeName(keySet, languageCode);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["refNo"] = refNo
            };

            return GetBetTypeName(keySet, languageCode);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId,
                ["refNo"] = refNo
            };

            return GetBetTypeName(keySet, languageCode) ?? defaultValue;
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId
            };

            return GetBetTypeName(keySet, languageCode) ?? defaultValue;
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["refNo"] = refNo
            };

            return GetBetTypeName(keySet, languageCode) ?? defaultValue;
        }

        private string GetBetTypeName(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.BetTypeName, "betTypeName", keySet, languageCode);

        public string GetCountryName(string contryCode, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["contryCode"] = contryCode
            };

            return GetTranslationText(RefDataType.CountryName, "countryName", keySet, languageCode) ?? defaultValue;
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId)
            => GetKenoBetChoice(kenoBetChoiceId, languageCode, string.Empty, out kenoBetTypeId);

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId)
        {
            var keySet = new KeySet
            {
                ["kenoBetChoiceId"] = kenoBetChoiceId
            };

            return GetKenoBetChoice(keySet, languageCode, out kenoBetTypeId) ?? defaultValue;
        }

        private string GetKenoBetChoice(KeySet keySet, string languageCode, out string kenoBetTypeId)
        {
            const string actionName = "kenoBetChoice";
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.KenoBetChoice,
                parameters => GetTranslationSet(RefController + actionName, parameters),
                parameterSets => SyncTranslationSets(RefSyncController + actionName, parameterSets));
            var kenoChoicesAndBetTypes = handler.Get(keySet, languageCode) as Tuple<string, string>;

            kenoBetTypeId = kenoChoicesAndBetTypes.Item2;

            return kenoChoicesAndBetTypes.Item1;
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBettype(string kenoBetTypeId, string languageCode)
            => GetKenoBettype(kenoBetTypeId, languageCode, string.Empty);

        public string GetKenoBettype(string kenoBetTypeId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["kenoBetTypeId"] = kenoBetTypeId
            };

            return GetKenoBettype(keySet, languageCode) ?? defaultValue;
        }

        private string GetKenoBettype(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.KenoBetType, "kenoBetType", keySet, languageCode);

        public string GetLeagueName(string leagueId)
            => GetLeagueName(leagueId, RefDataTypeHandlerOptions.DefaultLanguage, string.Empty);

        public string GetLeagueName(string leagueId, string languageCode)
            => GetLeagueName(leagueId, languageCode, string.Empty);

        public string GetLeagueName(string leagueId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["leagueId"] = leagueId
            };

            return GetLeagueName(keySet, languageCode) ?? defaultValue;
        }

        private string GetLeagueName(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.LeagueName, "leagueName", keySet, languageCode);

        public string GetParentIdByBetTypeId(string betTypeId, string defaultValue)
            => GetParentIdByBetTypeId(betTypeId, RefDataTypeHandlerOptions.DefaultLanguage, defaultValue);

        public string GetParentIdByBetTypeId(string betTypeId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };

            return GetParentIdByBetTypeId(keySet, languageCode) ?? defaultValue;
        }

        private string GetParentIdByBetTypeId(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.ParentBetTypeId, "parentIdByBetTypeId", keySet, languageCode);

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["refId"] = refId,
                ["resourceId"] = resourceId
            };

            return GetResourceDataByRefIdAndResourceId(keySet, languageCode) ?? defaultValue;
        }

        private string GetResourceDataByRefIdAndResourceId(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.RefResource, "resourceData", keySet, languageCode);

        public string GetSportName(string sportTypeId, string languageCode)
            => GetSportName(sportTypeId, languageCode, string.Empty);

        public string GetSportName(string sportTypeId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["sportTypeId"] = sportTypeId
            };

            return GetSportName(keySet, languageCode) ?? defaultValue;
        }

        private string GetSportName(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.SportName, "sportName", keySet, languageCode);

        public string GetTeamName(string teamId)
            => GetTeamName(teamId, RefDataTypeHandlerOptions.DefaultLanguage, string.Empty);

        public string GetTeamName(string teamId, string languageCode)
            => GetTeamName(teamId, languageCode, string.Empty);

        public string GetTeamName(string teamId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["teamId"] = teamId
            };

            return GetTeamName(keySet, languageCode) ?? defaultValue;
        }

        public string GetTeamName(string teamId, string languageCode, int sportTypeId)
        {
            var keySet = new KeySet
            {
                ["teamId"] = teamId,
                ["sportTypeId"] = sportTypeId
            };

            return GetTeamName(keySet, languageCode);
        }

        private string GetTeamName(KeySet keySet, string languageCode)
            => GetTranslationText(RefDataType.TeamName, "teamName", keySet, languageCode);

        private string GetTranslationText(RefDataType dataType, string actionName, KeySet keySet, string languageCode)
        {
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                dataType,
                parameters => GetTranslationSet(RefController + actionName, parameters),
                parameterSets => SyncTranslationSets(RefSyncController + actionName, parameterSets));
            object text = handler.Get(keySet, languageCode);

            return text?.ToString();
        }

        private TranslationSet GetTranslationSet(string path, KeySet parameters)
        {
            throw new NotImplementedException();
        }

        private async Task<IDictionary<string, TranslationSet>> SyncTranslationSets(string path, IList<KeySet> parameterSets)
        {
            throw new NotImplementedException();
        }
    }
}