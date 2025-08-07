#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)

namespace Fanex.RemotingRef;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fanex.RemotingRef.TranslationStorage;
using Fanex.RemotingRef.Utilities.Serialization;
using Fanex.RemotingRef.Utilities.WebClient;

public class RemotingRefService : IRemotingRefService
{
    private const string RefController = "api/ref/";
    private const string RefSyncController = "api/refsync/";

    private static readonly HashSet<string> LeagueGroupIdsNeedRemoveText =
    [
        "42", "74", "108", "113", "122", "151"
    ];

    private const int RegexTimeoutMilliseconds = 1000;

    private const string SabaRefId = "Saba";

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

    public string GetVirtualSportMatchCodeFormat(string matchCode, string refId, string languageCode)
    {
        if (string.IsNullOrEmpty(matchCode) || string.IsNullOrEmpty(refId))
        {
            return string.Empty;
        }

        var str1 = matchCode;
        var chArray = new[] { '{' };
        foreach (var str2 in str1.Split(chArray))
        {
            var length = str2.IndexOf('}');
            if (length != -1)
            {
                var resourceId = str2.Substring(0, length);
                var refIdAndResourceId = GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, string.Empty);
                matchCode = matchCode.Replace("{" + resourceId + "}", refIdAndResourceId);
            }
        }

        return matchCode;
    }

    public string GetBetTypeName(string betTypeId, string languageCode)
        => GetBetTypeNameInternal(
            betTypeId,
            null,
            null,
            languageCode,
            string.Empty);

    public string GetBetTypeName(string betTypeId, string languageCode, long refNo)
        => GetBetTypeNameInternal(
            betTypeId,
            null,
            refNo,
            languageCode,
            string.Empty);

    public string GetBetTypeName(
        string betTypeId,
        string languageCode,
        long refNo,
        string defaultValue)
        => GetBetTypeNameInternal(
            betTypeId,
            null,
            refNo,
            languageCode,
            defaultValue);

    public string GetBetTypeName(
        string betTypeId,
        string languageCode,
        string defaultValue,
        string resourceId)
        => GetBetTypeNameInternal(
            betTypeId,
            resourceId,
            null,
            languageCode,
            defaultValue);

    public string GetBetTypeName(
        string betTypeId,
        string languageCode,
        string defaultValue,
        string resourceId,
        long refNo)
        => GetBetTypeNameInternal(
            betTypeId,
            resourceId,
            refNo,
            languageCode,
            defaultValue);

    private string GetBetTypeNameInternal(
        string betTypeId,
        string resourceId,
        long? refNo,
        string languageCode,
        string defaultValue)
    {
        if (IsUnacceptableNumericId(betTypeId))
        {
            return defaultValue;
        }

        const string actionName = "BetTypeName";

        var keySet = new KeySet
        {
            [nameof(betTypeId)] = betTypeId
        };

        if (resourceId != null)
        {
            keySet[nameof(resourceId)] = resourceId;
        }

        if (refNo.HasValue)
        {
            keySet[nameof(refNo)] = refNo.Value;
        }

        var betTypeName = GetTranslationText(
            RefDataType.BetTypeName,
            actionName,
            keySet,
            languageCode);

        return betTypeName ?? defaultValue;
    }

    public string GetSportName(string sportTypeId, string languageCode)
        => GetSportName(sportTypeId, languageCode, string.Empty);

    public string GetSportName(string sportTypeId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(sportTypeId))
        {
            return defaultValue;
        }

        const string actionName = "SportName";

        var keySet = new KeySet
        {
            [nameof(sportTypeId)] = sportTypeId
        };
        var sportName = GetTranslationText(
            RefDataType.SportName,
            actionName,
            keySet,
            languageCode);

        return sportName ?? defaultValue;
    }

    public string GetTeamName(string teamId, string languageCode)
        => GetTeamName(teamId, languageCode, string.Empty);

    public string GetTeamName(string teamId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(teamId))
        {
            return defaultValue;
        }

        const string actionName = "TeamName";

        var keySet = new KeySet
        {
            [nameof(teamId)] = teamId
        };
        var teamName = GetTranslationText(
            RefDataType.TeamName,
            actionName,
            keySet,
            languageCode);

        return teamName ?? defaultValue;
    }

    public string GetTeamName3rd(string team3rdId, string languageCode)
        => GetTeamName3rd(team3rdId, languageCode, string.Empty);

    public string GetTeamName3rd(string team3rdId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(team3rdId))
        {
            return defaultValue;
        }

        const string actionName = "TeamName3rd";

        var keySet = new KeySet
        {
            [nameof(team3rdId)] = team3rdId
        };
        var teamName = GetTranslationText(
            RefDataType.TeamName3rd,
            actionName,
            keySet,
            languageCode);

        return teamName ?? defaultValue;
    }

    public string GetLeagueName(string leagueId, string languageCode)
        => GetLeagueName(leagueId, languageCode, string.Empty);

    public string GetLeagueName(string leagueId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(leagueId))
        {
            return defaultValue;
        }

        const string actionName = "LeagueName";

        var keySet = new KeySet
        {
            [nameof(leagueId)] = leagueId
        };
        var leagueName = GetTranslationText(
            RefDataType.LeagueName,
            actionName,
            keySet,
            languageCode);

        return leagueName ?? defaultValue;
    }

    public string GetLeagueName(string leagueId, string leagueGroupId, string languageCode, string defaultValue)
    {
        var leagueName = GetLeagueName(leagueId, languageCode, defaultValue);

        if (LeagueGroupIdsNeedRemoveText.Contains(leagueGroupId))
        {
            try
            {
#pragma warning disable S4142 // Duplicate values should not be passed as arguments
                var sabaWord = GetResourceDataByRefIdAndResourceId(SabaRefId, SabaRefId, languageCode, SabaRefId);
#pragma warning restore S4142 // Duplicate values should not be passed as arguments
                if (!string.IsNullOrEmpty(sabaWord))
                {
                    var sabaWords = new[] { $" {sabaWord}", $"{sabaWord} ", sabaWord };
                    leagueName = RemoveSabaWordsWithTimeout(leagueName, sabaWords);
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return leagueName;
            }
        }

        return leagueName;
    }

    public string GetLeagueName3rd(string league3rdId, string languageCode)
        => GetLeagueName3rd(league3rdId, languageCode, string.Empty);

    public string GetLeagueName3rd(string league3rdId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(league3rdId))
        {
            return defaultValue;
        }

        const string actionName = "LeagueName3rd";

        var keySet = new KeySet
        {
            [nameof(league3rdId)] = league3rdId
        };
        var leagueName = GetTranslationText(
            RefDataType.LeagueName3rd,
            actionName,
            keySet,
            languageCode);

        return leagueName ?? defaultValue;
    }

    public string GetCountryName(string countryCode, string languageCode)
        => GetCountryName(countryCode, languageCode, string.Empty);

    public string GetCountryName(string countryCode, string languageCode, string defaultValue)
    {
        if (IsUnacceptableTextualId(countryCode))
        {
            return defaultValue;
        }

        const string actionName = "CountryName";

        var keySet = new KeySet
        {
            [nameof(countryCode)] = countryCode
        };
        var countryName = GetTranslationText(
            RefDataType.CountryName,
            actionName,
            keySet,
            languageCode);

        return countryName ?? defaultValue;
    }

    public string GetOddsTypeName(string oddsTypeId, string languageCode)
        => GetOddsTypeName(oddsTypeId, languageCode, string.Empty);

    public string GetOddsTypeName(string oddsTypeId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(oddsTypeId))
        {
            return defaultValue;
        }

        const string actionName = "OddsTypeName";

        var keySet = new KeySet
        {
            [nameof(oddsTypeId)] = oddsTypeId
        };
        var oddsTypeName = GetTranslationText(
            RefDataType.OddsTypeName,
            actionName,
            keySet,
            languageCode);

        return oddsTypeName ?? defaultValue;
    }

    public string GetResourceDataByRefIdAndResourceId(
        string refId,
        string resourceId,
        string languageCode,
        string defaultValue)
    {
        if (IsUnacceptableTextualId(refId) || IsUnacceptableTextualId(resourceId))
        {
            return defaultValue;
        }

        const string actionName = "ResourceData";

        var keySet = new KeySet
        {
            [nameof(refId)] = refId,
            [nameof(resourceId)] = resourceId
        };
        var resourceData = GetTranslationText(
            RefDataType.RefResource,
            actionName,
            keySet,
            languageCode);

        return resourceData ?? defaultValue;
    }

    public string GetParentIdByBetTypeId(string betTypeId, string defaultValue)
    {
        if (IsUnacceptableNumericId(betTypeId))
        {
            return defaultValue;
        }

        const string actionName = "ParentIdByBetTypeId";

        var keySet = new KeySet
        {
            [nameof(betTypeId)] = betTypeId
        };
        var parentBetTypeId = GetTranslationText(
            RefDataType.ParentBetTypeId,
            actionName,
            keySet,
            null);

        return parentBetTypeId ?? defaultValue;
    }

    public string[] GetCombinationTypeCombo(
        string betTeam,
        string betCheck,
        int handicap1,
        int handicap2,
        string languageCode)
    {
        const string actionName = "CombinationTypeComboParlay";

        var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
            RefDataType.CombinationTypeComboParlay,
            parameters => RequestTranslationAsync<string[]>(actionName, parameters),
            parameterSets => SyncTranslationsAsync<string[]>(actionName, parameterSets));
        var keySet = new KeySet
        {
            [nameof(betTeam)] = betTeam,
            [nameof(betCheck)] = betCheck,
            [nameof(handicap1)] = handicap1,
            [nameof(handicap2)] = handicap2
        };

        return handler.Get(keySet, languageCode) as string[];
    }

    public string GetHorseTeamName(string codeId, string languageCode)
        => GetHorseTeamName(codeId, languageCode, string.Empty);

    public string GetHorseTeamName(string codeId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(codeId))
        {
            return defaultValue;
        }

        const string actionName = "HorseTeamName";

        var keySet = new KeySet
        {
            [nameof(codeId)] = codeId
        };
        var horseTeamName = GetTranslationText(
            RefDataType.HorseTeamName,
            actionName,
            keySet,
            languageCode);

        return horseTeamName ?? defaultValue;
    }

    public string GetCasinoBetItem(string itemId, string languageCode)
        => GetCasinoBetItem(itemId, languageCode, string.Empty);

    public string GetCasinoBetItem(string itemId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableTextualId(itemId))
        {
            return defaultValue;
        }

        const string actionName = "CasinoBetItem";

        var keySet = new KeySet
        {
            [nameof(itemId)] = itemId
        };
        var casinoBetItem = GetTranslationText(
            RefDataType.CasinoBetItem,
            actionName,
            keySet,
            null);

        return casinoBetItem ?? defaultValue;
    }

    public string GetRouletteBetItem(string itemId, string languageCode)
        => GetRouletteBetItem(itemId, languageCode, string.Empty);

    public string GetRouletteBetItem(string itemId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableTextualId(itemId))
        {
            return defaultValue;
        }

        const string actionName = "RouletteBetItem";

        var keySet = new KeySet
        {
            [nameof(itemId)] = itemId
        };
        var rouletteBetItem = GetTranslationText(
            RefDataType.RouletteBetItem,
            actionName,
            keySet,
            languageCode);

        return rouletteBetItem ?? defaultValue;
    }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

    public string GetKenoBetChoice(string kenoBetChoiceId, string languageCode, out string kenoBetTypeId)
        => GetKenoBetChoice(
            kenoBetChoiceId,
            languageCode,
            string.Empty,
            out kenoBetTypeId);

    public string GetKenoBetChoice(
        string kenoBetChoiceId,
        string languageCode,
        string defaultValue,
        out string kenoBetTypeId)
    {
        if (IsUnacceptableNumericId(kenoBetChoiceId))
        {
            kenoBetTypeId = defaultValue;

            return defaultValue;
        }

        const string actionName = "KenoBetChoice";

        var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
            RefDataType.KenoBetChoice,
            parameters => RequestTranslationAsync<Tuple<string, string>>(actionName, parameters),
            parameterSets => SyncTranslationsAsync<Tuple<string, string>>(actionName, parameterSets));
        var keySet = new KeySet
        {
            [nameof(kenoBetChoiceId)] = kenoBetChoiceId
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
        if (IsUnacceptableNumericId(kenoBetTypeId))
        {
            return defaultValue;
        }

        const string actionName = "KenoBetType";

        var keySet = new KeySet
        {
            [nameof(kenoBetTypeId)] = kenoBetTypeId
        };
        var kenoBetTypeName = GetTranslationText(
            RefDataType.KenoBetType,
            actionName,
            keySet,
            languageCode);

        return kenoBetTypeName ?? defaultValue;
    }

    public string GetFastMarketsChoice(
        string betTypeId,
        string betTeam,
        int handicap1,
        int handicap2,
        string languageCode)
    {
        if (IsUnacceptableNumericId(betTypeId) || IsUnacceptableTextualId(betTeam))
        {
            return string.Empty;
        }

        const string actionName = "FastMarketsChoice";

        var keySet = new KeySet
        {
            [nameof(betTypeId)] = betTypeId,
            [nameof(betTeam)] = betTeam,
            [nameof(handicap1)] = handicap1,
            [nameof(handicap2)] = handicap2
        };

        var fastMarketsChoice = GetTranslationText(
            RefDataType.FastMarketsChoice,
            actionName,
            keySet,
            languageCode);

        return fastMarketsChoice ?? string.Empty;
    }

    public string GetFastMarketsResult(string resultId, string languageCode)
    {
        if (IsUnacceptableNumericId(resultId))
        {
            return string.Empty;
        }

        const string actionName = "FastMarketsResult";

        var keySet = new KeySet
        {
            [nameof(resultId)] = resultId
        };
        var fastMarketsResult = GetTranslationText(
            RefDataType.FastMarketsResult,
            actionName,
            keySet,
            languageCode);

        return fastMarketsResult ?? string.Empty;
    }

    public string GetFastMarkets225Result(string resultId, string languageCode)
    {
        if (IsUnacceptableNumericId(resultId))
        {
            return string.Empty;
        }

        const string actionName = "FastMarkets225Result";

        var keySet = new KeySet
        {
            [nameof(resultId)] = resultId
        };
        var fastMarkets255Result = GetTranslationText(
            RefDataType.FastMarkets225Result,
            actionName,
            keySet,
            languageCode);

        return fastMarkets255Result ?? string.Empty;
    }

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

    public string GetLotteryBetChoice(string lotteryBetChoiceId, string languageCode, out string lotteryBetTypeId)
        => GetLotteryBetChoice(
            lotteryBetChoiceId,
            languageCode,
            string.Empty,
            out lotteryBetTypeId);

    public string GetLotteryBetChoice(
        string lotteryBetChoiceId,
        string languageCode,
        string defaultValue,
        out string lotteryBetTypeId)
    {
        if (IsUnacceptableNumericId(lotteryBetChoiceId))
        {
            lotteryBetTypeId = defaultValue;

            return defaultValue;
        }

        const string actionName = "LotteryBetChoice";

        var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
            RefDataType.LotteryBetChoice,
            parameters => RequestTranslationAsync<Tuple<string, string>>(actionName, parameters),
            parameterSets => SyncTranslationsAsync<Tuple<string, string>>(actionName, parameterSets));
        var keySet = new KeySet
        {
            [nameof(lotteryBetChoiceId)] = lotteryBetChoiceId
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
        if (IsUnacceptableNumericId(lotteryBetTypeId))
        {
            return defaultValue;
        }

        const string actionName = "LotteryBetType";

        var keySet = new KeySet
        {
            [nameof(lotteryBetTypeId)] = lotteryBetTypeId
        };
        var lotteryBetTypeName = GetTranslationText(
            RefDataType.LotteryBetType,
            actionName,
            keySet,
            languageCode);

        return lotteryBetTypeName ?? defaultValue;
    }

    public string GetBetRadarMatchCodeFormat(string matchCode, string languageCode)
    {
        if (IsUnacceptableTextualId(matchCode))
        {
            return matchCode;
        }

        const string actionName = "BetRadarMatchCodeFormat";

        var keySet = new KeySet
        {
            [nameof(matchCode)] = matchCode
        };
        var matchCodeFormat = GetTranslationText(
            RefDataType.BetRadarMatchCodeFormat,
            actionName,
            keySet,
            languageCode);

        return matchCodeFormat ?? matchCode;
    }

    public string GetDisplayedTypeId(string betTypeId)
    {
        if (IsUnacceptableNumericId(betTypeId))
        {
            return betTypeId;
        }

        const string actionName = "DisplayedTypeId";

        var keySet = new KeySet
        {
            [nameof(betTypeId)] = betTypeId
        };
        var displayedTypeId = GetTranslationText(
            RefDataType.DisplayedTypeId,
            actionName,
            keySet,
            null);

        return displayedTypeId ?? betTypeId;
    }

    public string GetDisplayedBetTypeName(string betTypeId, string languageCode)
        => GetDisplayedBetTypeNameInternal(
            betTypeId,
            null,
            null,
            languageCode,
            string.Empty);

    public string GetDisplayedBetTypeName(string betTypeId, long refNo, string languageCode)
        => GetDisplayedBetTypeNameInternal(
            betTypeId,
            null,
            refNo,
            languageCode,
            string.Empty);

    public string GetDisplayedBetTypeName(
        string betTypeId,
        long refNo,
        string languageCode,
        string defaultValue)
        => GetDisplayedBetTypeNameInternal(
            betTypeId,
            null,
            refNo,
            languageCode,
            defaultValue);

    public string GetDisplayedBetTypeName(
        string betTypeId,
        string resourceId,
        string languageCode,
        string defaultValue)
        => GetDisplayedBetTypeNameInternal(
            betTypeId,
            resourceId,
            null,
            languageCode,
            defaultValue);

    public string GetDisplayedBetTypeName(
        string betTypeId,
        string resourceId,
        long refNo,
        string languageCode,
        string defaultValue)
        => GetDisplayedBetTypeNameInternal(
            betTypeId,
            resourceId,
            refNo,
            languageCode,
            defaultValue);

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

    public string GetTableGamesBetChoice(string tableGamesBetChoiceId, string languageCode, out string tableGamesBetTypeId)
        => GetTableGamesBetChoice(
            tableGamesBetChoiceId,
            languageCode,
            string.Empty,
            out tableGamesBetTypeId);

    public string GetTableGamesBetChoice(
        string tableGamesBetChoiceId,
        string languageCode,
        string defaultValue,
        out string tableGamesBetTypeId)
    {
        if (IsUnacceptableNumericId(tableGamesBetChoiceId))
        {
            tableGamesBetTypeId = defaultValue;

            return defaultValue;
        }

        const string actionName = "TableGamesBetChoice";

        var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
            RefDataType.TableGamesBetChoice,
            parameters => RequestTranslationAsync<Tuple<string, string>>(actionName, parameters),
            parameterSets => SyncTranslationsAsync<Tuple<string, string>>(actionName, parameterSets));
        var keySet = new KeySet
        {
            [nameof(tableGamesBetChoiceId)] = tableGamesBetChoiceId
        };
        var choiceAndBetType = handler.Get(keySet, languageCode) as Tuple<string, string>;

        tableGamesBetTypeId = choiceAndBetType?.Item2 ?? defaultValue;

        return choiceAndBetType?.Item1 ?? defaultValue;
    }

#pragma warning restore S3874 // "out" and "ref" parameters should not be used

    public string GetTableGamesBetType(string tableGamesBetTypeId, string languageCode)
        => GetTableGamesBetType(tableGamesBetTypeId, languageCode, string.Empty);

    public string GetTableGamesBetType(string tableGamesBetTypeId, string languageCode, string defaultValue)
    {
        if (IsUnacceptableNumericId(tableGamesBetTypeId))
        {
            return defaultValue;
        }

        const string actionName = "TableGamesBetType";

        var keySet = new KeySet
        {
            [nameof(tableGamesBetTypeId)] = tableGamesBetTypeId
        };
        var tableGamesBetTypeName = GetTranslationText(
            RefDataType.TableGamesBetType,
            actionName,
            keySet,
            languageCode);

        return tableGamesBetTypeName ?? defaultValue;
    }

    private string GetDisplayedBetTypeNameInternal(
        string betTypeId,
        string resourceId,
        long? refNo,
        string languageCode,
        string defaultValue)
    {
        if (IsUnacceptableNumericId(betTypeId))
        {
            return defaultValue;
        }

        const string actionName = "DisplayedBetTypeName";

        var keySet = new KeySet
        {
            [nameof(betTypeId)] = betTypeId
        };

        if (resourceId != null)
        {
            keySet[nameof(resourceId)] = resourceId;
        }

        if (refNo.HasValue)
        {
            keySet[nameof(refNo)] = refNo.Value;
        }

        var displayedBetTypeName = GetTranslationText(
            RefDataType.DisplayedBetTypeName,
            actionName,
            keySet,
            languageCode);

        return displayedBetTypeName ?? defaultValue;
    }

    public string[] GetBetBuilderSelectionNames(int[] selectionIds, string languageCode)
    {
        var betBuilderSelectionNames = new List<string>();
        if (selectionIds == null || selectionIds.Length == 0)
        {
            return [.. betBuilderSelectionNames];
        }

        foreach (var selectionId in selectionIds)
        {
            var betBuilderSelectionName = GetBetBuilderSelectionNameInternal(selectionId, languageCode);

            if (!string.IsNullOrEmpty(betBuilderSelectionName))
            {
                betBuilderSelectionNames.Add(betBuilderSelectionName);
            }
        }

        return [.. betBuilderSelectionNames];
    }

    private string GetBetBuilderSelectionNameInternal(int selectionId, string languageCode)
    {
        if (selectionId == 0)
        {
            return default;
        }

        const string actionName = "BetBuilderSelectionName";

        var keySet = new KeySet
        {
            [nameof(selectionId)] = selectionId
        };
        var betBuilderSelectionName = GetTranslationText(
            RefDataType.BetBuilderSelectionName,
            actionName,
            keySet,
            languageCode);

        return betBuilderSelectionName;
    }

    private static bool IsUnacceptableNumericId(string id)
        => id == "0" || string.IsNullOrEmpty(id);

    private static bool IsUnacceptableTextualId(string id)
        => string.IsNullOrEmpty(id);

    private string GetTranslationText(
        RefDataType dataType,
        string actionName,
        KeySet keySet,
        string languageCode)
        => GetTranslationObject<string>(
            dataType,
            actionName,
            keySet,
            languageCode);

    private T GetTranslationObject<T>(
        RefDataType dataType,
        string actionName,
        KeySet keySet,
        string languageCode)
    {
        var handler = RefDataTypeContainer.TryResolveDataTypeHandler(
            dataType,
            parameters => RequestTranslationAsync<T>(actionName, parameters),
            parameterSets => SyncTranslationsAsync<T>(actionName, parameterSets));
        var result = (T)handler.Get(keySet, languageCode);

        return result;
    }

    private static string BuildRequestRelativePath(string path, KeySet parameters)
    {
        var relativePathBuilder = new StringBuilder($"{RefController}{path}?");

        foreach (var parameter in parameters.Where(parameter => parameter.Value != null))
        {
            relativePathBuilder.Append($"{parameter.Key}={Uri.EscapeDataString(parameter.Value.ToString())}&");
        }

        foreach (var language in RefDataTypeHandlerOptions.SupportedLanguageCodes)
        {
            relativePathBuilder.Append($"languages={language}&");
        }

        relativePathBuilder.Append($"applicationName={RefDataTypeHandlerOptions.ApplicationName}");

        return relativePathBuilder.ToString();
    }

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used

    private async Task<TranslationSet> RequestTranslationAsync<T>(string action, KeySet parameters)
    {
        var requestRelativePath = BuildRequestRelativePath(action, parameters);
        var response = await _client.GetAsync(requestRelativePath);

        if (response.IsSuccessStatusCode)
        {
            var responseResult = await response.Content.ReadAsStreamAsync();
            var result = await _serializer.DeserializeAsync<Dictionary<string, T>>(responseResult);

            return TranslationSet.FromDictionary(result);
        }

        throw new RemotingRefException(
            response.RequestMessage.RequestUri,
            response.StatusCode,
            response.ReasonPhrase);
    }

    private async Task<IDictionary<string, TranslationSet>> SyncTranslationsAsync<T>(string action, IList<KeySet> parameterSets)
    {
        var formData = new
        {
            ParameterSets = parameterSets,
            Languages = RefDataTypeHandlerOptions.SupportedLanguageCodes
        };
        var requestContent = new ByteArrayContent(_serializer.Serialize(formData));
        var response = await _client.PostAsync(RefSyncController + action, requestContent);

        if (response.IsSuccessStatusCode)
        {
            var responseResult = await response.Content.ReadAsStreamAsync();
            var result = await _serializer.DeserializeAsync<Dictionary<KeySet, Dictionary<string, T>>>(responseResult);

            return result.ToDictionary(
                pair => pair.Key.BuildKey(),
                pair => TranslationSet.FromDictionary(pair.Value));
        }

        return null;
    }

    private static string RemoveSabaWordsWithTimeout(string leagueName, string[] sabaWords)
    {
        foreach (var word in sabaWords)
        {
            try
            {
                leagueName = Regex.Replace(
                    leagueName, 
                    Regex.Escape(word),
                    string.Empty, 
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(RegexTimeoutMilliseconds));
            }
            catch (RegexMatchTimeoutException)
            {
#pragma warning disable S3626 // Jump statements should not be redundant
                continue;
#pragma warning restore S3626 // Jump statements should not be redundant
            }
        }

        return leagueName;
    }

#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
}