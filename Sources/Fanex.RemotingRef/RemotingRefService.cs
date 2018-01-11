namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.TranslationStorage;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Fanex.RemotingRef.Utilities.WebClient;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)

    public class RemotingRefService : IRemotingRefService
    {
        private const string RefController = "/api/ref/";
        private const string RefSyncController = "/api/refsync/";
        private readonly IHttpHandler _client;
        private readonly ISerializer _serializer;

        public RemotingRefService(IHttpHandler client = null, ISerializer serializer = null)
        {
            _client = client ?? new Utilities.WebClient.HttpClientHandler(RefDataTypeContainer.ServiceUri);
            _serializer = serializer ?? new MessagePackSerializer();
        }

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

            return GetBetTypeName(keySet, languageCode, "BetTypeNameWithRefNo");
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue)
        {
            string betTypeName = GetBetTypeName(betTypeId, languageCode, refNo);

            return string.IsNullOrEmpty(betTypeName) ? defaultValue : betTypeName;
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId
            };
            string betTypeName = GetBetTypeName(keySet, languageCode, "BetTypeNameWithResourceId");

            return string.IsNullOrEmpty(betTypeName) ? defaultValue : betTypeName;
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId,
                ["refNo"] = refNo
            };
            string betTypeName = GetBetTypeName(keySet, languageCode, "BetTypeNameWithRefNoAndResourceId");

            return string.IsNullOrEmpty(betTypeName) ? defaultValue : betTypeName;
        }

        private string GetBetTypeName(KeySet keySet, string languageCode, string actionName = "BetTypeName")
            => GetTranslationText(RefDataType.BetTypeName, actionName, keySet, languageCode);

        public string GetSportName(string sportTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["sportTypeId"] = sportTypeId
            };

            return GetTranslationText(RefDataType.SportName, "sportName", keySet, languageCode);
        }

        public string GetSportName(string sportTypeId, string languageCode, string defaultValue)
        {
            string sportName = GetSportName(sportTypeId, languageCode);

            return string.IsNullOrEmpty(sportName) ? defaultValue : sportName;
        }

        public string GetTeamName(string teamId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["teamId"] = teamId
            };

            return GetTranslationText(RefDataType.TeamName, "teamName", keySet, languageCode);
        }

        public string GetTeamName(string teamId, string languageCode, string defaultValue)
        {
            string teamName = GetTeamName(teamId, languageCode);

            return string.IsNullOrEmpty(teamName) ? defaultValue : teamName;
        }

        public string GetTeamName3rd(string team3rdId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["team3rdId"] = team3rdId
            };

            return GetTranslationText(RefDataType.TeamName3rd, "teamName3rd", keySet, languageCode);
        }

        public string GetTeamName3rd(string team3rdId, string languageCode, string defaultValue)
        {
            string teamName3rd = GetTeamName3rd(team3rdId, languageCode);

            return string.IsNullOrEmpty(teamName3rd) ? defaultValue : teamName3rd;
        }

        public string GetLeagueName(string leagueId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["leagueId"] = leagueId
            };

            return GetTranslationText(RefDataType.LeagueName, "leagueName", keySet, languageCode);
        }

        public string GetLeagueName(string leagueId, string languageCode, string defaultValue)
        {
            string leagueName = GetLeagueName(leagueId, languageCode);

            return string.IsNullOrEmpty(leagueName) ? defaultValue : leagueName;
        }

        public string GetLeagueName3rd(string league3rdId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["league3rdId"] = league3rdId
            };

            return GetTranslationText(RefDataType.LeagueName3rd, "leagueName3rd", keySet, languageCode);
        }

        public string GetLeagueName3rd(string league3rdId, string languageCode, string defaultValue)
        {
            string leagueName3rd = GetLeagueName3rd(league3rdId, languageCode);

            return string.IsNullOrEmpty(leagueName3rd) ? defaultValue : leagueName3rd;
        }

        public string GetCountryName(string countryCode, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["countryCode"] = countryCode
            };
            string countryName = GetTranslationText(RefDataType.CountryName, "countryName", keySet, languageCode);

            return string.IsNullOrEmpty(countryName) ? defaultValue : countryName;
        }

        public string GetOddsTypeName(string oddsTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["oddsTypeId"] = oddsTypeId
            };

            return GetTranslationText(RefDataType.OddsTypeName, "oddsTypeName", keySet, languageCode);
        }

        public string GetOddsTypeName(string oddsTypeId, string languageCode, string defaultValue)
        {
            string oddsTypeName = GetOddsTypeName(oddsTypeId, languageCode);

            return string.IsNullOrEmpty(oddsTypeName) ? defaultValue : oddsTypeName;
        }

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["refId"] = refId,
                ["resourceId"] = resourceId
            };
            string resource = GetTranslationText(RefDataType.RefResource, "resourceData", keySet, languageCode);

            return string.IsNullOrEmpty(resource) ? defaultValue : resource;
        }

        public string GetParentIdByBetTypeId(string betTypeId, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };
            string parentBetTypeId = GetTranslationText(RefDataType.ParentBetTypeId, "parentIdByBetTypeId", keySet, null);

            return string.IsNullOrEmpty(parentBetTypeId) ? defaultValue : parentBetTypeId;
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string languageCode)
        {
            const string actionName = "combinationTypeCombo";
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.LotteryBetChoice,
                parameters => RequestTranslation<string[]>(RefController + actionName, parameters),
                parameterSets => SyncTranslationsAsync<string[]>(RefSyncController + actionName, parameterSets));
            var keySet = new KeySet
            {
                ["betTeam"] = betTeam,
                ["betCheck"] = betCheck,
                ["handicap1"] = handicap1,
                ["handicap2"] = handicap2,
            };

            return handler.Get(keySet, null) as string[];
        }

        public string GetHorseTeamName(string codeId, string languageCode, string defaultValue)
            => throw new NotSupportedException();

        public string GetCasinoBetItem(string itemId, string languageCode, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["itemId"] = itemId
            };
            string casinoBetItem = GetTranslationText(RefDataType.CasinoBetItem, "casinoBetItem", keySet, null);

            return string.IsNullOrEmpty(casinoBetItem) ? defaultValue : casinoBetItem;
        }

        public string GetRouletteBetItem(string itemId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["itemId"] = itemId
            };

            return GetTranslationText(RefDataType.RouletteBetItem, "rouletteBetItem", keySet, languageCode);
        }

        public string GetRouletteBetItem(string itemId, string languageCode, string defaultValue)
        {
            string rouletteBetItem = GetRouletteBetItem(itemId, languageCode);

            return string.IsNullOrEmpty(rouletteBetItem) ? defaultValue : rouletteBetItem;
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId)
        {
            var keySet = new KeySet
            {
                ["kenoBetChoiceId"] = kenoBetChoiceId
            };

            return GetKenoBetChoice(keySet, languageCode, out kenoBetTypeId);
        }

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId)
        {
            string kenoBetChoice = GetKenoBetChoice(kenoBetChoiceId, languageCode, out kenoBetTypeId);

            return string.IsNullOrEmpty(kenoBetChoice) ? defaultValue : kenoBetChoice;
        }

        private string GetKenoBetChoice(KeySet keySet, string languageCode, out string kenoBetTypeId)
        {
            const string actionName = "kenoBetChoice";
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.KenoBetChoice,
                parameters => RequestTranslation<Tuple<string, string>>(RefController + actionName, parameters),
                parameterSets => SyncTranslationsAsync<Tuple<string, string>>(RefSyncController + actionName, parameterSets));
            var kenoChoicesAndBetTypes = handler.Get(keySet, languageCode) as Tuple<string, string>;

            kenoBetTypeId = kenoChoicesAndBetTypes.Item2;

            return kenoChoicesAndBetTypes.Item1;
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["kenoBetTypeId"] = kenoBetTypeId
            };

            return GetTranslationText(RefDataType.KenoBetType, "kenoBetType", keySet, languageCode);
        }

        public string GetKenoBetType(string kenoBetTypeId, string languageCode, string defaultValue)
        {
            string kenoBetType = GetKenoBetType(kenoBetTypeId, languageCode);

            return string.IsNullOrEmpty(kenoBetType) ? defaultValue : kenoBetType;
        }

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["betTeam"] = betTeam,
                ["handicap1"] = handicap1,
                ["handicap2"] = handicap2,
            };

            return GetTranslationText(RefDataType.FastMarketsChoice, "fastMarketsChoice", keySet, languageCode);
        }

        public string GetFastMarkets225Result(string resultId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["resultId"] = resultId
            };

            return GetTranslationText(RefDataType.FastMarkets225Result, "fastMarkets225Result", keySet, languageCode);
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, out string lotteryBetTypeId)
        {
            var keySet = new KeySet
            {
                ["betChoiceId"] = lotteryBetChoiceId
            };

            return GetLotteryBetChoice(keySet, languageCode, out lotteryBetTypeId);
        }

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, string defaultValue, out string lotteryBetTypeId)
        {
            string lotteryBetChoice = GetLotteryBetChoice(lotteryBetChoiceId, languageCode, out lotteryBetTypeId);

            return string.IsNullOrEmpty(lotteryBetChoice) ? defaultValue : lotteryBetChoice;
        }

        private string GetLotteryBetChoice(KeySet keySet, string languageCode, out string lotteryBetTypeId)
        {
            const string actionName = "lotteryBetChoice";
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.LotteryBetChoice,
                parameters => RequestTranslation<Tuple<string, string>>(RefController + actionName, parameters),
                parameterSets => SyncTranslationsAsync<Tuple<string, string>>(RefSyncController + actionName, parameterSets));
            var lotteryChoicesAndBetTypes = handler.Get(keySet, languageCode) as Tuple<string, string>;

            lotteryBetTypeId = lotteryChoicesAndBetTypes.Item2;

            return lotteryChoicesAndBetTypes.Item1;
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = lotteryBetTypeId
            };

            return GetTranslationText(RefDataType.LotteryBetType, "lotteryBetType", keySet, languageCode);
        }

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode, string defaultValue)
        {
            string lotteryBetType = GetLotteryBetType(lotteryBetTypeId, languageCode);

            return string.IsNullOrEmpty(lotteryBetType) ? defaultValue : lotteryBetType;
        }

        private string GetTranslationText(RefDataType dataType, string actionName, KeySet keySet, string languageCode)
            => GetTranslationObject<string>(dataType, actionName, keySet, languageCode);

        private T GetTranslationObject<T>(RefDataType dataType, string actionName, KeySet keySet, string languageCode)
        {
            IRefDataTypeHandler handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                dataType,
                parameters => RequestTranslation<T>(RefController + actionName, parameters),
                parameterSets => SyncTranslationsAsync<T>(RefSyncController + actionName, parameterSets));
            T result = (T)handler.Get(keySet, languageCode);

            return result;
        }

        private TranslationSet RequestTranslation<T>(string path, KeySet parameters)
        {
            var result = new Dictionary<string, T>();
            var requestUri = new StringBuilder($"{path}?");

            foreach (var parameter in parameters)
            {
                requestUri.Append($"{parameter.Key}={parameter.Value}&");
            }

            foreach (var language in RefDataTypeHandlerOptions.SupportedLanguageCodes)
            {
                requestUri.Append($"languages={language}&");
            }

            var response = _client.GetAsync(requestUri.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseResult = response.Content.ReadAsStreamAsync().Result;

                result = _serializer.Deserialize<Dictionary<string, T>>(responseResult);
            }

            return TranslationSet.FromDictionary(result);
        }

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used

        private async Task<IDictionary<string, TranslationSet>> SyncTranslationsAsync<T>(string path, IList<KeySet> parameterSets)
        {
            var result = new Dictionary<KeySet, Dictionary<string, T>>();

            var formData = new RequestSyncFormData
            {
                ParameterSets = parameterSets,
                Languages = RefDataTypeHandlerOptions.SupportedLanguageCodes
            };

            var byteFormData = new ByteArrayContent(_serializer.Serialize(formData));

            var response = await _client.PostAsync(path, byteFormData);

            if (response.IsSuccessStatusCode)
            {
                var responseResult = response.Content.ReadAsStreamAsync().Result;

                result = _serializer.Deserialize<Dictionary<KeySet, Dictionary<string, T>>>(responseResult);
            }

            return result.ToDictionary(pair => pair.Key.BuildKey(), pair => TranslationSet.FromDictionary(pair.Value));
        }

#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
    }
}

#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)