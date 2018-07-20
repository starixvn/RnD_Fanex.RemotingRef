#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
#pragma warning disable S3994 // URI Parameters should not be strings
#pragma warning disable S3995 // URI Parameters should not be strings

namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.RemotingRef.TranslationStorage;
    using Fanex.RemotingRef.Utilities.Serialization;
    using Fanex.RemotingRef.Utilities.WebClient;

    public class RemotingRefService : IRemotingRefService
    {
        private const string RefController = "api/ref/";
        private const string RefSyncController = "api/refsync/";
        private readonly IHttpHandler _client;
        private readonly ISerializer _serializer;

        public RemotingRefService()
            : this(null, null)
        {
        }

        internal RemotingRefService(IHttpHandler client, ISerializer serializer)
        {
            _client = client ?? new MsgPackHttpClientHandler();
            _serializer = serializer ?? new MessagePackSerializer();
        }

        public string GetBetTypeName(string betTypeId, string languageCode)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };

            return GetBetTypeName(keySet, languageCode, string.Empty);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo)
            => GetBetTypeName(betTypeId, languageCode, refNo, string.Empty);

        public string GetBetTypeName(string betTypeId, string languageCode, long refNo, string defaultValue)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["refNo"] = refNo
            };

            return GetBetTypeName(keySet, languageCode, defaultValue);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId
            };

            return GetBetTypeName(keySet, languageCode, defaultValue);
        }

        public string GetBetTypeName(string betTypeId, string languageCode, string defaultValue, string resourceId, long refNo)
        {
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["resourceId"] = resourceId,
                ["refNo"] = refNo
            };

            return GetBetTypeName(keySet, languageCode, defaultValue);
        }

        private string GetBetTypeName(KeySet keySet, string languageCode, string defaultValue)
            => GetTranslationText(RefDataType.BetTypeName, "BetTypeName", keySet, languageCode) ?? defaultValue;

        public string GetSportName(string sportTypeId, string languageCode)
            => GetSportName(sportTypeId, languageCode, string.Empty);

        public string GetSportName(string sportTypeId, string languageCode, string defaultValue)
        {
            const string actionName = "SportName";
            var keySet = new KeySet
            {
                ["sportTypeId"] = sportTypeId
            };
            var sportName = GetTranslationText(RefDataType.SportName, actionName, keySet, languageCode);

            return sportName ?? defaultValue;
        }

        public string GetTeamName(string teamId, string languageCode)
            => GetTeamName(teamId, languageCode, string.Empty);

        public string GetTeamName(string teamId, string languageCode, string defaultValue)
        {
            const string actionName = "TeamName";
            var keySet = new KeySet
            {
                ["teamId"] = teamId
            };
            var teamName = GetTranslationText(RefDataType.TeamName, actionName, keySet, languageCode);

            return teamName ?? defaultValue;
        }

        public string GetTeamName3rd(string team3rdId, string languageCode)
            => GetTeamName3rd(team3rdId, languageCode, string.Empty);

        public string GetTeamName3rd(string team3rdId, string languageCode, string defaultValue)
        {
            const string actionName = "TeamName3rd";
            var keySet = new KeySet
            {
                ["team3rdId"] = team3rdId
            };
            var teamName3rd = GetTranslationText(RefDataType.TeamName3rd, actionName, keySet, languageCode);

            return teamName3rd ?? defaultValue;
        }

        public string GetLeagueName(string leagueId, string languageCode)
            => GetLeagueName(leagueId, languageCode, string.Empty);

        public string GetLeagueName(string leagueId, string languageCode, string defaultValue)
        {
            const string actionName = "LeagueName";
            var keySet = new KeySet
            {
                ["leagueId"] = leagueId
            };
            var leagueName = GetTranslationText(RefDataType.LeagueName, actionName, keySet, languageCode);

            return leagueName ?? defaultValue;
        }

        public string GetLeagueName3rd(string league3rdId, string languageCode)
            => GetLeagueName3rd(league3rdId, languageCode, string.Empty);

        public string GetLeagueName3rd(string league3rdId, string languageCode, string defaultValue)
        {
            const string actionName = "LeagueName3rd";
            var keySet = new KeySet
            {
                ["league3rdId"] = league3rdId
            };
            var leagueName3rd = GetTranslationText(RefDataType.LeagueName3rd, actionName, keySet, languageCode);

            return leagueName3rd ?? defaultValue;
        }

        public string GetCountryName(string countryCode, string languageCode)
            => GetCountryName(countryCode, languageCode, string.Empty);

        public string GetCountryName(string countryCode, string languageCode, string defaultValue)
        {
            const string actionName = "CountryName";
            var keySet = new KeySet
            {
                ["countryCode"] = countryCode
            };
            var countryName = GetTranslationText(RefDataType.CountryName, actionName, keySet, languageCode);

            return countryName ?? defaultValue;
        }

        public string GetOddsTypeName(string oddsTypeId, string languageCode)
            => GetOddsTypeName(oddsTypeId, languageCode, string.Empty);

        public string GetOddsTypeName(string oddsTypeId, string languageCode, string defaultValue)
        {
            const string actionName = "OddsTypeName";
            var keySet = new KeySet
            {
                ["oddsTypeId"] = oddsTypeId
            };
            var oddsTypeName = GetTranslationText(RefDataType.OddsTypeName, actionName, keySet, languageCode);

            return oddsTypeName ?? defaultValue;
        }

        public string GetResourceDataByRefIdAndResourceId(string refId, string resourceId, string languageCode, string defaultValue)
        {
            const string actionName = "ResourceData";
            var keySet = new KeySet
            {
                ["refId"] = refId,
                ["resourceId"] = resourceId
            };
            var resource = GetTranslationText(RefDataType.RefResource, actionName, keySet, languageCode);

            return resource ?? defaultValue;
        }

        public string GetParentIdByBetTypeId(string betTypeId, string defaultValue)
        {
            const string actionName = "ParentIdByBetTypeId";
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };
            var parentBetTypeId = GetTranslationText(RefDataType.ParentBetTypeId, actionName, keySet, null);

            return parentBetTypeId ?? defaultValue;
        }

        public string[] GetCombinationTypeCombo(string betTeam, string betCheck, int handicap1, int handicap2, string languageCode)
        {
            const string actionName = "CombinationTypeComboParlay";
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.CombinationTypeCombo,
                parameters => RequestTranslation<string[]>(actionName, parameters),
                parameterSets => SyncTranslationsAsync<string[]>(actionName, parameterSets));
            var keySet = new KeySet
            {
                ["betTeam"] = betTeam,
                ["betCheck"] = betCheck,
                ["handicap1"] = handicap1,
                ["handicap2"] = handicap2,
            };

            return handler.Get(keySet, languageCode) as string[];
        }

        public string GetHorseTeamName(string codeId, string languageCode)
            => GetHorseTeamName(codeId, languageCode, string.Empty);

        public string GetHorseTeamName(string codeId, string languageCode, string defaultValue)
        {
            const string actionName = "HorseTeamName";
            var keySet = new KeySet
            {
                ["codeId"] = codeId
            };
            var horseTeamName = GetTranslationText(RefDataType.HorseTeamName, actionName, keySet, languageCode);

            return horseTeamName ?? defaultValue;
        }

        public string GetCasinoBetItem(string itemId, string languageCode)
            => GetCasinoBetItem(itemId, languageCode, string.Empty);

        public string GetCasinoBetItem(string itemId, string languageCode, string defaultValue)
        {
            const string actionName = "CasinoBetItem";
            var keySet = new KeySet
            {
                ["itemId"] = itemId
            };
            var casinoBetItem = GetTranslationText(RefDataType.CasinoBetItem, actionName, keySet, null);

            return casinoBetItem ?? defaultValue;
        }

        public string GetRouletteBetItem(string itemId, string languageCode)
            => GetRouletteBetItem(itemId, languageCode, string.Empty);

        public string GetRouletteBetItem(string itemId, string languageCode, string defaultValue)
        {
            const string actionName = "RouletteBetItem";
            var keySet = new KeySet
            {
                ["itemId"] = itemId
            };
            var rouletteBetItem = GetTranslationText(RefDataType.RouletteBetItem, actionName, keySet, languageCode);

            return rouletteBetItem ?? defaultValue;
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId)
            => GetKenoBetChoice(kenoBetChoiceId, languageCode, string.Empty, out kenoBetTypeId);

        public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, string defaultValue, out string kenoBetTypeId)
        {
            const string actionName = "KenoBetChoice";
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.KenoBetChoice,
                parameters => RequestTranslation<Tuple<string, string>>(actionName, parameters),
                parameterSets => SyncTranslationsAsync<Tuple<string, string>>(actionName, parameterSets));
            var keySet = new KeySet
            {
                ["kenoBetChoiceId"] = kenoBetChoiceId
            };
            var choiceAndBetType = handler.Get(keySet, languageCode) as Tuple<string, string>;

            kenoBetTypeId = choiceAndBetType?.Item2 ?? defaultValue;

            return choiceAndBetType?.Item1 ?? defaultValue;
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetKenoBetType(string kenoBetTypeId, string languageCode)
            => GetKenoBetType(kenoBetTypeId, languageCode, string.Empty);

        public string GetKenoBetType(string kenoBetTypeId, string languageCode, string defaultValue)
        {
            const string actionName = "KenoBetType";
            var keySet = new KeySet
            {
                ["kenoBetTypeId"] = kenoBetTypeId
            };
            var kenoBetType = GetTranslationText(RefDataType.KenoBetType, actionName, keySet, languageCode);

            return kenoBetType ?? defaultValue;
        }

        public string GetFastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, string languageCode)
        {
            const string actionName = "FastMarketsChoice";
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId,
                ["betTeam"] = betTeam,
                ["handicap1"] = handicap1,
                ["handicap2"] = handicap2,
            };
            var fastMarketChoice = GetTranslationText(RefDataType.FastMarketsChoice, actionName, keySet, languageCode);

            return fastMarketChoice ?? string.Empty;
        }

        public string GetFastMarkets225Result(string resultId, string languageCode)
        {
            const string actionName = "FastMarkets225Result";
            var keySet = new KeySet
            {
                ["resultId"] = resultId
            };
            var fastMarketResult = GetTranslationText(RefDataType.FastMarkets225Result, actionName, keySet, languageCode);

            return fastMarketResult ?? string.Empty;
        }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, out string lotteryBetTypeId)
            => GetLotteryBetChoice(lotteryBetChoiceId, languageCode, string.Empty, out lotteryBetTypeId);

        public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, string defaultValue, out string lotteryBetTypeId)
        {
            const string actionName = "LotteryBetChoice";
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                RefDataType.LotteryBetChoice,
                parameters => RequestTranslation<Tuple<string, string>>(actionName, parameters),
                parameterSets => SyncTranslationsAsync<Tuple<string, string>>(actionName, parameterSets));
            var keySet = new KeySet
            {
                ["betChoiceId"] = lotteryBetChoiceId
            };
            var choiceAndBetType = handler.Get(keySet, languageCode) as Tuple<string, string>;

            lotteryBetTypeId = choiceAndBetType?.Item2 ?? defaultValue;

            return choiceAndBetType?.Item1 ?? defaultValue;
        }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode)
            => GetLotteryBetType(lotteryBetTypeId, languageCode, string.Empty);

        public string GetLotteryBetType(string lotteryBetTypeId, string languageCode, string defaultValue)
        {
            const string actionName = "LotteryBetType";
            var keySet = new KeySet
            {
                ["betTypeId"] = lotteryBetTypeId
            };
            var lotteryBetType = GetTranslationText(RefDataType.LotteryBetType, actionName, keySet, languageCode);

            return lotteryBetType ?? defaultValue;
        }

        public string GetBetRadarMatchCodeFormat(string matchCode, string languageCode)
        {
            const string actionName = "BetRadarMatchCodeFormat";
            var keySet = new KeySet
            {
                ["matchCode"] = matchCode
            };
            var matchCodeFormat = GetTranslationText(RefDataType.BetRadarMatchCodeFormat, actionName, keySet, languageCode);

            return matchCodeFormat ?? matchCode;
        }

        public string GetDisplayedTypeId(string betTypeId)
        {
            const string actionName = "DisplayedTypeId";
            var keySet = new KeySet
            {
                ["betTypeId"] = betTypeId
            };
            var displayedTypeId = GetTranslationText(RefDataType.DisplayedTypeId, actionName, keySet, null);

            return displayedTypeId ?? betTypeId;
        }

        private string GetTranslationText(RefDataType dataType, string actionName, KeySet keySet, string languageCode)
            => GetTranslationObject<string>(dataType, actionName, keySet, languageCode);

        private T GetTranslationObject<T>(RefDataType dataType, string actionName, KeySet keySet, string languageCode)
        {
            var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
                dataType,
                parameters => RequestTranslation<T>(actionName, parameters),
                parameterSets => SyncTranslationsAsync<T>(actionName, parameterSets));
            var result = (T)handler.Get(keySet, languageCode);

            return result;
        }

        private TranslationSet RequestTranslation<T>(string action, KeySet parameters)
        {
            var requestUrl = BuildRequestUrl(action, parameters);
            var response = _client.GetAsync(requestUrl).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var responseResult = response.Content?.ReadAsStreamAsync().GetAwaiter().GetResult();
                var result = _serializer.Deserialize<Dictionary<string, T>>(responseResult);

                return TranslationSet.FromDictionary(result);
            }

            throw new RemotingRefException(requestUrl, response.StatusCode, response.ReasonPhrase);
        }

        private static string BuildRequestUrl(string path, KeySet parameters)
        {
            var urlBuilder = new StringBuilder($"{RefController}{path}?");

            foreach (var parameter in parameters)
            {
                if (parameter.Value != null)
                {
                    urlBuilder.Append($"{parameter.Key}={Uri.EscapeDataString(parameter.Value.ToString())}&");
                }
            }

            foreach (var language in RefDataTypeHandlerOptions.SupportedLanguageCodes)
            {
                urlBuilder.Append($"languages={language}&");
            }

            urlBuilder.Append($"applicationName={RefDataTypeHandlerOptions.ApplicationName}");

            return urlBuilder.ToString();
        }

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used

        private async Task<IDictionary<string, TranslationSet>> SyncTranslationsAsync<T>(string action, IList<KeySet> parameterSets)
        {
            var formData = new ByteArrayContent(_serializer.Serialize(new
            {
                ParameterSets = parameterSets,
                Languages = RefDataTypeHandlerOptions.SupportedLanguageCodes,
                ApplicationName = RefDataTypeHandlerOptions.ApplicationName
            }));
            var response = await _client.PostAsync(RefSyncController + action, formData);

            if (response.IsSuccessStatusCode)
            {
                var responseResult = await response.Content.ReadAsStreamAsync();
                var result = await _serializer.DeserializeAsync<Dictionary<KeySet, Dictionary<string, T>>>(responseResult);

                return result.ToDictionary(pair => pair.Key.BuildKey(), pair => TranslationSet.FromDictionary(pair.Value));
            }

            return null;
        }

#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
    }
}