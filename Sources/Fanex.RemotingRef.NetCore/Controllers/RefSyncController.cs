namespace Fanex.RemotingRef.NetCore
{
    using Fanex.RemotingRef.NetCore.Delegates;
    using Fanex.RemotingRef.NetCore.Models;
    using Fanex.RemotingRef.NetCore.Services;
    using Fanex.RemotingRef.NetCore.Services.Interface;
    using Fanex.RemotingRef.NetCore.Utilities;
    using Fanex.RemotingRef.NetCore.Utilities.RefData;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]")]
    public class RefSyncController(
            IRefDataHelper refData,
            ITeam3rdService team3rdService,
            ILeague3rdService league3rdService,
            IBettypeService bettypeService,
            ISportService sportService,
            IRefResourceService refResourceService,
            AppSettings appSettings) : Controller
    {
        private readonly IRefDataHelper _refData = refData;

        private readonly ITeam3rdService _team3rdService = team3rdService;
        private readonly ILeague3rdService _league3rdService = league3rdService;
        private readonly IBettypeService _bettypeService = bettypeService;
        private readonly ISportService _sportService = sportService;
        private readonly IRefResourceService _refResourceService = refResourceService;
        private readonly AppSettings _appSettings = appSettings;

        [HttpPost("BetTypeName")]
        public SyncedTranslationResultSet BetTypeName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _appSettings.UsingVNFunction ?
                    _bettypeService.GetBettypeById(parameterSet.GetBetTypeId(), parameterSet.GetResourceId(), parameterSet.GetRefNo(), refLanguageCode) :
                    _refData.GetBetTypeName(parameterSet.GetBetTypeId(), parameterSet.GetResourceId(), parameterSet.GetRefNo(), refLanguageCode));

        [HttpPost("SportName")]
        public SyncedTranslationResultSet SportName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _appSettings.UsingVNFunction ?
                                            _sportService.GetSportById(parameterSet.GetSportTypeId(), refLanguageCode) :
                                            _refData.GetSportName(parameterSet.GetSportTypeId(), refLanguageCode));

        [HttpPost("TeamName")]
        public SyncedTranslationResultSet TeamName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetTeamName(parameterSet.GetTeamId(), refLanguageCode));

        [HttpPost("TeamName3rd")]
        public SyncedTranslationResultSet TeamName3rd(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _team3rdService.GetTeam3rdById(parameterSet.GetTeam3rdId(), refLanguageCode));

        [HttpPost("LeagueName")]
        public SyncedTranslationResultSet LeagueName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetLeagueName(parameterSet.GetLeagueId(), refLanguageCode));

        [HttpPost("LeagueName3rd")]
        public SyncedTranslationResultSet LeagueName3rd(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _league3rdService.GetLeague3rdById(parameterSet.GetLeague3rdId(), refLanguageCode));

        [HttpPost("CountryName")]
        public SyncedTranslationResultSet CountryName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetCountryName(parameterSet.GetCountryCode(), refLanguageCode));

        [HttpPost("OddsTypeName")]
        public SyncedTranslationResultSet OddsTypeName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetOddsTypeName(parameterSet.GetOddsTypeId(), refLanguageCode));

        [HttpPost("ResourceData")]
        public SyncedTranslationResultSet ResourceData(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _appSettings.UsingVNFunction ?
                    _refResourceService.GetRefResourceById(parameterSet.GetRefId(), parameterSet.GetResourceId(), refLanguageCode) :
                    _refData.GetResourceDataByRefIdAndResourceId(parameterSet.GetRefId(), parameterSet.GetResourceId(), refLanguageCode));

        [HttpPost("ParentIdByBetTypeId")]
        public SyncedTranslationResultSet ParentIdByBetTypeId(RequestSyncFormData formData)
            => GetSyncedNeutralTextSet(
                formData.ParameterSets,
                parameterSet => _refData.GetParentIdByBetTypeId(parameterSet.GetBetTypeId()));

        [HttpPost("CombinationTypeComboParlay")]
        public SyncedTranslationResultSet CombinationTypeComboParlay(RequestSyncFormData formData)
        {
            bool VerifyComboList(List<string> combos)
                => combos != null && combos.Exists(combo => !string.IsNullOrEmpty(combo));

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetCombinationTypeCombo(
                    parameterSet.GetBetTeam(),
                    parameterSet.GetBetCheck(),
                    parameterSet.GetHandicap1(),
                    parameterSet.GetHandicap2(),
                    refLanguageCode),
                VerifyComboList);
        }

        [HttpPost("HorseTeamName")]
        public SyncedTranslationResultSet HorseTeamName(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetHorseTeamName(parameterSet.GetCodeId(), refLanguageCode));

        [HttpPost("CasinoBetItem")]
        public SyncedTranslationResultSet CasinoBetItem(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetCasinoBetItem(parameterSet.GetItemId(), refLanguageCode));

        [HttpPost("RouletteBetItem")]
        public SyncedTranslationResultSet RouletteBetItem(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetRouletteBetItem(parameterSet.GetItemId(), refLanguageCode));

        [HttpPost("KenoBetChoice")]
        public SyncedTranslationResultSet KenoBetChoice(RequestSyncFormData formData)
        {
            Tuple<string, string> GetKenoBetChoiceAndType(ParameterSet parameterSet, string refLanguageCode)
            {
                var (name, id) = _refData.GetKenoBetChoice(
                    parameterSet.GetKenoBetChoiceId(),
                    refLanguageCode);

                return new Tuple<string, string>(name, id);
            }

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                GetKenoBetChoiceAndType,
                choiceAndTypePair => !string.IsNullOrEmpty(choiceAndTypePair.Item2));
        }

        [HttpPost("KenoBetType")]
        public SyncedTranslationResultSet KenoBetType(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetKenoBetType(parameterSet.GetKenoBetTypeId(), refLanguageCode));

        [HttpPost("FastMarketsChoice")]
        public SyncedTranslationResultSet FastMarketsChoice(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetFastMarketsChoice(
                    parameterSet.GetBetTypeId(),
                    parameterSet.GetBetTeam(),
                    parameterSet.GetHandicap1(),
                    parameterSet.GetHandicap2(),
                    refLanguageCode));

        [HttpPost("FastMarketsResult")]
        public SyncedTranslationResultSet FastMarketsResult(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetFastMarketsResult(parameterSet.GetResultId(), refLanguageCode));

        [HttpPost("FastMarkets225Result")]
        public SyncedTranslationResultSet FastMarkets225Result(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetFastMarkets225Result(parameterSet.GetResultId(), refLanguageCode));

        [HttpPost("LotteryBetChoice")]
        public SyncedTranslationResultSet LotteryBetChoice(RequestSyncFormData formData)
        {
            Tuple<string, string> GetLotteryBetChoiceAndType(ParameterSet parameterSet, string refLanguageCode)
            {
                var lotteryBetChoiceId = parameterSet.GetLotteryBetChoiceId() ?? parameterSet.GetBetChoiceId();
                var (name, id) = _refData.GetLotteryBetChoice(
                    lotteryBetChoiceId,
                    refLanguageCode);

                return new Tuple<string, string>(name, id);
            }

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                GetLotteryBetChoiceAndType,
                choiceAndTypePair => !string.IsNullOrEmpty(choiceAndTypePair.Item1));
        }

        [HttpPost("LotteryBetType")]
        public SyncedTranslationResultSet LotteryBetType(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) =>
                {
                    var lotteryBetTypeId = parameterSet.GetLotteryBetTypeId() ?? parameterSet.GetBetTypeId();

                    return _refData.GetLotteryBetType(lotteryBetTypeId, refLanguageCode);
                });

        [HttpPost("BetRadarMatchCodeFormat")]
        public SyncedTranslationResultSet BetRadarMatchCodeFormat(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetBetRadarMatchCodeFormat(parameterSet.GetMatchCode(), refLanguageCode));

        [HttpPost("DisplayedTypeId")]
        public SyncedTranslationResultSet DisplayedTypeId(RequestSyncFormData formData)
            => GetSyncedNeutralTextSet(
                formData.ParameterSets,
                parameterSet => _refData.GetDisplayedTypeId(parameterSet.GetBetTypeId()));

        [HttpPost("DisplayedBetTypeName")]
        public SyncedTranslationResultSet DisplayedBetTypeName(RequestSyncFormData formData)
        {
            var syncedResultSet = new SyncedTranslationResultSet();
            var standardLanguageCodes = formData.Languages;

            foreach (var parameterSet in formData.ParameterSets)
            {
                var displayedBetTypeId = _refData.GetDisplayedTypeId(parameterSet.GetBetTypeId());
                var resourceId = parameterSet.GetResourceId();
                var refNo = parameterSet.GetRefNo();
                var translationSet = new TranslationSet();

                for (var counter = 0; counter < standardLanguageCodes.Length; counter++)
                {
                    var displayedBetTypeNameInSpecificLanguage = _appSettings.UsingVNFunction ?
                                _bettypeService.GetBettypeById(displayedBetTypeId, resourceId, refNo, standardLanguageCodes[counter]) :
                                _refData.GetBetTypeName(displayedBetTypeId, resourceId, refNo, standardLanguageCodes[counter]);

                    if (!string.IsNullOrEmpty(displayedBetTypeNameInSpecificLanguage))
                    {
                        translationSet.Add(standardLanguageCodes[counter], displayedBetTypeNameInSpecificLanguage);
                    }
                }

                if (translationSet.Count > 0)
                {
                    syncedResultSet.Add(parameterSet, translationSet);
                }
            }

            return syncedResultSet;
        }

        [HttpPost("BetBuilderSelectionName")]
        public SyncedTranslationResultSet BetBuilderSelectionName(RequestSyncFormData formData)
        {
            return GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetBetBuilderSelectionName(parameterSet.GetSelectionId(), refLanguageCode));
        }

        [HttpPost("TableGamesBetChoice")]
        public SyncedTranslationResultSet TableGamesBetChoice(RequestSyncFormData formData)
        {
            Tuple<string, string> GetTableGamesBetChoiceAndType(ParameterSet parameterSet, string refLanguageCode)
            {
                var (name, id) = _refData.GetTableGamesBetChoice(
                    parameterSet.GetTableGamesBetChoiceId(),
                    refLanguageCode);

                return new Tuple<string, string>(name, id);
            }

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                GetTableGamesBetChoiceAndType,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost("TableGamesBetType")]
        public SyncedTranslationResultSet TableGamesBetType(RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetKenoBetType(parameterSet.GetTableGamesBetTypeId(), refLanguageCode));

        private static SyncedTranslationResultSet GetSyncedNeutralTextSet(IList<ParameterSet> parameterSets, SyncNeutralResouceDelegate<string> textGetter)
        {
            var syncedResultSet = new SyncedTranslationResultSet();

            foreach (var parameterSet in parameterSets)
            {
                var text = textGetter(parameterSet);

                if (!string.IsNullOrEmpty(text))
                {
                    syncedResultSet.Add(
                        parameterSet,
                        new TranslationSet
                        {
                            [LanguageHelpers.DefaultLanguage] = text
                        });
                }
            }

            return syncedResultSet;
        }

        private static SyncedTranslationResultSet GetSyncedTranslatedTextResultSet(
            IList<ParameterSet> parameterSets,
            string[] standardLanguageCodes,
            SyncTranslationDelegate<string> textGetter)
            => GetSyncedTranslationResultSet(
                parameterSets,
                standardLanguageCodes,
                textGetter,
                text => !string.IsNullOrEmpty(text));

        private static SyncedTranslationResultSet GetSyncedTranslationResultSet<T>(
            IList<ParameterSet> parameterSets,
            string[] standardLanguageCodes,
            SyncTranslationDelegate<T> translationGetter,
            VerifyTranslationDelegate<T> verifier)
        {
            var syncedResultSet = new SyncedTranslationResultSet();

            foreach (var parameterSet in parameterSets)
            {
                var translationSet = new TranslationSet();

                for (var counter = 0; counter < standardLanguageCodes.Length; counter++)
                {
                    var resourceInSpecificLanguage = translationGetter(parameterSet, standardLanguageCodes[counter]);

                    if (verifier(resourceInSpecificLanguage))
                    {
                        translationSet.Add(standardLanguageCodes[counter], resourceInSpecificLanguage);
                    }
                }

                if (translationSet.Count > 0)
                {
                    syncedResultSet.Add(parameterSet, translationSet);
                }
            }

            return syncedResultSet;
        }
    }
}