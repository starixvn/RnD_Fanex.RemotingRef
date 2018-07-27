namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Delegates;
    using Fanex.RemotingRef.Service.Models;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;

    public class RefSyncController : ApiController
    {
        private readonly IRefDataHelper _refData;

        public RefSyncController()
            : this(null)
        {
        }

        public RefSyncController(IRefDataHelper refData)
            => _refData = refData ?? new RefDataHelper();

        [HttpPost]
        public SyncedTranslationResultSet BetTypeName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetBetTypeName(
                    parameterSet.GetBetTypeId(),
                    parameterSet.GetResourceId(),
                    parameterSet.GetRefNo(),
                    refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet SportName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetSportName(parameterSet.GetSportTypeId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet TeamName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetTeamName(parameterSet.GetTeamId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet TeamName3rd([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetTeamName3rd(parameterSet.GetTeam3rdId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet LeagueName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetLeagueName(parameterSet.GetLeagueId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet LeagueName3rd([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetLeagueName3rd(parameterSet.GetLeague3rdId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet CountryName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetCountryName(parameterSet.GetCountryCode(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet OddsTypeName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetOddsTypeName(parameterSet.GetOddsTypeId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet ResourceData([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetResourceDataByRefIdAndResourceId(
                    parameterSet.GetRefId(),
                    parameterSet.GetResourceId(),
                    refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet ParentIdByBetTypeId([FromBody]RequestSyncFormData formData)
            => GetSyncedNeutralTextSet(
                formData.ParameterSets,
                parameterSet => _refData.GetParentIdByBetTypeId(parameterSet.GetBetTypeId()));

        [HttpPost]
        public SyncedTranslationResultSet CombinationTypeComboParlay([FromBody]RequestSyncFormData formData)
        {
            bool VerifyComboList(string[] combos)
                => combos != null && combos.Any(combo => !string.IsNullOrEmpty(combo));

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

        [HttpPost]
        public SyncedTranslationResultSet HorseTeamName([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetHorseTeamName(parameterSet.GetCodeId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet CasinoBetItem([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetCasinoBetItem(parameterSet.GetItemId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet RouletteBetItem([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetRouletteBetItem(parameterSet.GetItemId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet KenoBetChoice([FromBody] RequestSyncFormData formData)
        {
            Tuple<string, string> GetKenoBetChoiceAndType(ParameterSet parameterSet, string refLanguageCode)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(
                    parameterSet.GetKenoBetChoiceId(),
                    refLanguageCode,
                    out string kenoBetTypeId);

                if (string.IsNullOrEmpty(kenoBetChoice) && string.IsNullOrEmpty(kenoBetTypeId))
                {
                    return null;
                }

                return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
            }

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                GetKenoBetChoiceAndType,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost]
        public SyncedTranslationResultSet KenoBetType([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetKenoBetType(parameterSet.GetKenoBetTypeId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet FastMarketsChoice([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetFastMarketsChoice(
                    parameterSet.GetBetTypeId(),
                    parameterSet.GetBetTeam(),
                    parameterSet.GetHandicap1(),
                    parameterSet.GetHandicap2(),
                    refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet FastMarkets225Result([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetFastMarkets225Result(parameterSet.GetResultId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet LotteryBetChoice([FromBody] RequestSyncFormData formData)
        {
            Tuple<string, string> GetLotteryBetChoiceAndType(ParameterSet parameterSet, string refLanguageCode)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(
                    parameterSet.GetBetChoiceId(),
                    refLanguageCode,
                    out string lotteryBetTypeId);

                if (string.IsNullOrEmpty(lotteryBetChoice) && string.IsNullOrEmpty(lotteryBetTypeId))
                {
                    return null;
                }

                return new Tuple<string, string>(lotteryBetChoice, lotteryBetTypeId);
            }

            return GetSyncedTranslationResultSet(
                formData.ParameterSets,
                formData.Languages,
                GetLotteryBetChoiceAndType,
                choiceAndTypePair => choiceAndTypePair != null);
        }

        [HttpPost]
        public SyncedTranslationResultSet LotteryBetType([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetLotteryBetType(parameterSet.GetBetTypeId(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet BetRadarMatchCodeFormat([FromBody]RequestSyncFormData formData)
            => GetSyncedTranslatedTextResultSet(
                formData.ParameterSets,
                formData.Languages,
                (parameterSet, refLanguageCode) => _refData.GetBetRadarMatchCodeFormat(parameterSet.GetMatchCode(), refLanguageCode));

        [HttpPost]
        public SyncedTranslationResultSet DisplayedTypeId([FromBody]RequestSyncFormData formData)
            => GetSyncedNeutralTextSet(
                formData.ParameterSets,
                parameterSet => _refData.GetDisplayedTypeId(parameterSet.GetBetTypeId()));

        [HttpPost]
        public SyncedTranslationResultSet DisplayedBetTypeName([FromBody]RequestSyncFormData formData)
        {
            var syncedResultSet = new SyncedTranslationResultSet();
            var standardLanguageCodes = formData.Languages;
            var refLanguageCodes = LanguageHelpers.ToRefLanguageCodes(standardLanguageCodes);

            foreach (var parameterSet in formData.ParameterSets)
            {
                var displayedBetTypeId = _refData.GetDisplayedTypeId(parameterSet.GetBetTypeId());
                var resourceId = parameterSet.GetResourceId();
                var refNo = parameterSet.GetRefNo();
                var translationSet = new TranslationSet();

                for (var counter = 0; counter < refLanguageCodes.Length; counter++)
                {
                    var displayedBetTypeNameInSpecificLanguage = _refData.GetBetTypeName(
                        displayedBetTypeId,
                        resourceId,
                        refNo,
                        refLanguageCodes[counter]);

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
            var refLanguageCodes = LanguageHelpers.ToRefLanguageCodes(standardLanguageCodes);

            foreach (var parameterSet in parameterSets)
            {
                var translationSet = new TranslationSet();

                for (var counter = 0; counter < refLanguageCodes.Length; counter++)
                {
                    var resourceInSpecificLanguage = translationGetter(parameterSet, refLanguageCodes[counter]);

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