namespace Fanex.RemotingRef.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Fanex.RemotingRef.Service.Utilities;
    using Fanex.RemotingRef.Service.Utilities.RefData;

    public class RefController : ApiController
    {
        private readonly IRefDataHelper _refData = new RefDataHelper();

        public RefController()
            : this(null)
        {
        }

        public RefController(IRefDataHelper refData)
        {
            _refData = refData ?? _refData;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeName(string betTypeId, [FromUri] string[] languages)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    betTypeId);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNo(string betTypeId, long refNo, [FromUri] string[] languages)
        {
            var betTypeNames = GetRefResult(
                   (parameters, language) => _refData.GetBetTypeName(parameters[0].ToString(), language, (long)parameters[1]),
                   IsValidRefData,
                   languages,
                   betTypeId,
                   refNo);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithResourceId(string betTypeId, string resourceId, [FromUri]string[] languages)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0].ToString(), language, parameters[1].ToString()),
                    IsValidRefData,
                    languages,
                    betTypeId,
                    resourceId ?? string.Empty);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNoAndResourceId(string betTypeId, string resourceId, long refNo, [FromUri]string[] languages)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0].ToString(), language, parameters[1].ToString(), (long)parameters[2]),
                    IsValidRefData,
                    languages,
                    betTypeId,
                    resourceId ?? string.Empty,
                    refNo);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> SportName(string sportTypeId, [FromUri]string[] languages)
        {
            var sportNames = GetRefResult(
                    (parameters, language) => _refData.GetSportName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    sportTypeId);

            return sportNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName(string teamId, [FromUri]string[] languages)
        {
            var teamNames = GetRefResult(
                    (parameters, language) => _refData.GetTeamName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    teamId);

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName3rd(string team3rdId, [FromUri]string[] languages)
        {
            var teamNames = GetRefResult(
                    (parameters, language) => _refData.GetTeamName3rd(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    team3rdId);

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName(string leagueId, [FromUri]string[] languages)
        {
            var leagueNames = GetRefResult(
                    (parameters, language) => _refData.GetLeagueName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    leagueId);

            return leagueNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName3rd(string league3rdId, [FromUri]string[] languages)
        {
            var league3rdNames = GetRefResult(
                    (parameters, language) => _refData.GetLeagueName3rd(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    league3rdId);

            return league3rdNames;
        }

        [HttpGet]
        public Dictionary<string, string> ResourceData(string refId, string resourceId, [FromUri]string[] languages)
        {
            var resourceData = GetRefResult(
                    (parameters, language) => _refData.GetResourceDataByRefIdAndResourceId(parameters[0].ToString(), parameters[1].ToString(), language),
                    IsValidRefData,
                    languages,
                    refId,
                    resourceId);

            return resourceData;
        }

        [HttpGet]
        public Dictionary<string, string> CountryName(string countryCode, [FromUri]string[] languages)
        {
            var countryNames = GetRefResult(
                    (parameters, language) => _refData.GetCountryName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    countryCode);

            return countryNames;
        }

        [HttpGet]
        public Dictionary<string, string> OddsTypeName(string oddsTypeId, [FromUri]string[] languages)
        {
            var oddsTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetOddsTypeName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    oddsTypeId);

            return oddsTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> ParentIdByBetTypeId(string betTypeId)
        {
            var parentId = GetRefResult(
                    (parameters, _) => _refData.GetParentIdByBetTypeId(parameters[0].ToString()),
                    IsValidRefData,
                    new[] { LanguageHelper.DefaultLanguage },
                    betTypeId);

            return parentId;
        }

        [HttpGet]
        public Dictionary<string, string[]> CombinationTypeComboParlay(
            string betTeam,
            string betCheck,
            int handicap1,
            int handicap2,
            [FromUri]string[] languages)
        {
            var combinationTypes = GetRefResult(
                    (parameters, language) => _refData.GetCombinationTypeCombo(parameters[0].ToString(), parameters[1].ToString(), (int)parameters[2], (int)parameters[3], language),
                    (refData) => refData.Any(item => !string.IsNullOrEmpty(item)),
                    languages,
                    betTeam,
                    betCheck,
                    handicap1,
                    handicap2);

            return combinationTypes;
        }

        [HttpGet]
        public Dictionary<string, string> HorseTeamName(string codeId, [FromUri]string[] languages)
        {
            var horseTeamNames = GetRefResult(
                    (parameters, language) => _refData.GetHorseTeamName(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    codeId);

            return horseTeamNames;
        }

        [HttpGet]
        public Dictionary<string, string> CasinoBetItem(string itemId, [FromUri]string[] languages)
        {
            var casinoBetItems = GetRefResult(
                    (parameters, language) => _refData.GetCasinoBetItem(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    itemId);

            return casinoBetItems;
        }

        [HttpGet]
        public Dictionary<string, string> RouletteBetItem(string itemId, [FromUri]string[] languages)
        {
            var rouletteBetItems = GetRefResult(
                    (parameters, language) => _refData.GetRouletteBetItem(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    itemId);

            return rouletteBetItems;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> KenoBetChoice(string kenoBetChoiceId, [FromUri]string[] languages)
        {
            Tuple<string, string> getKenoBetChoice(object[] parameters, string language)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(parameters[0].ToString(), language, out string kenoBetTypeId);

                return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
            }

            var kenoBetChoices = GetRefResult(
                    getKenoBetChoice,
                    (refData) => !string.IsNullOrEmpty(refData.Item1),
                    languages,
                    kenoBetChoiceId);

            return kenoBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> KenoBetType(string kenoBetTypeId, [FromUri]string[] languages)
        {
            var kenoBetTypes = GetRefResult(
                    (parameters, language) => _refData.GetKenoBetType(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    kenoBetTypeId);

            return kenoBetTypes;
        }

        [HttpGet]
        public Dictionary<string, string> FastMarketsChoice(string betTypeId, string betTeam, int handicap1, int handicap2, [FromUri]string[] languages)
        {
            var fastMarketsChoices = GetRefResult(
                    (parameters, language) => _refData.GetFastMarketsChoice(
                        parameters[0].ToString(),
                        parameters[1].ToString(),
                        (int)parameters[2],
                        (int)parameters[3],
                        language),
                    IsValidRefData,
                    languages,
                    betTypeId,
                    betTeam,
                    handicap1,
                    handicap2);

            return fastMarketsChoices;
        }

        [HttpGet]
        public Dictionary<string, string> FastMarkets225Result(string resultId, [FromUri]string[] languages)
        {
            var fastMarkets225Results = GetRefResult(
                    (parameters, language) => _refData.GetFastMarkets225Result(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    resultId);

            return fastMarkets225Results;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> LotteryBetChoice(string betChoiceId, [FromUri]string[] languages)
        {
            Tuple<string, string> getLotteryBetChoice(object[] parameters, string language)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(parameters[0].ToString(), language, out string lotteryBetTypeID);

                return new Tuple<string, string>(lotteryBetChoice, lotteryBetTypeID);
            }

            var lotteryBetChoices = GetRefResult(
                    getLotteryBetChoice,
                    (refData) => !string.IsNullOrEmpty(refData.Item1),
                    languages,
                    betChoiceId);

            return lotteryBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> LotteryBetType(string betTypeId, [FromUri]string[] languages)
        {
            var lotteryBetTypes = GetRefResult(
                    (parameters, language) => _refData.GetLotteryBetType(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    betTypeId);

            return lotteryBetTypes;
        }

        [HttpGet]
        public Dictionary<string, string> BetRadarMatchCodeFormat(string matchCode, [FromUri]string[] languages)
        {
            var refResult = GetRefResult(
                    (parameters, language) => _refData.GetBetRadarMatchCodeFormat(parameters[0].ToString(), language),
                    IsValidRefData,
                    languages,
                    matchCode);

            return refResult;
        }

        [HttpGet]
        public Dictionary<string, string> DisplayedTypeId(string betTypeId)
        {
            var refResult = GetRefResult(
                    (parameters, _) => _refData.GetDisplayedTypeId(parameters[0].ToString()),
                    IsValidRefData,
                    new[] { LanguageHelper.DefaultLanguage },
                    betTypeId);

            return refResult;
        }

        private static Dictionary<string, T> GetRefResult<T>(
            Func<object[], string, T> getRefData,
            Func<T, bool> isValidResultData,
            string[] languages,
            params object[] parameters)
        {
            var refResult = new Dictionary<string, T>();

            foreach (var language in languages)
            {
                var refData = getRefData(parameters, LanguageHelper.GenerateTaiwanLanguage(language));

                if (isValidResultData(refData))
                {
                    refResult.Add(language, refData);
                }
            }

            return refResult.Count > 0 ? refResult : null;
        }

        private static bool IsValidRefData(string refData) => !string.IsNullOrEmpty(refData);
    }
}