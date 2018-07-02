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
        public Dictionary<string, string> BetTypeName(string betTypeId = null, [FromUri] string[] languages = null)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    betTypeId);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNo(string betTypeId = null, long refNo = 0, [FromUri] string[] languages = null)
        {
            var betTypeNames = GetRefResult(
                   (parameters, language) => _refData.GetBetTypeName(parameters[0] as string, language, (long)parameters[1]),
                   IsValidRefData,
                   languages,
                   betTypeId,
                   refNo);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithResourceId(string betTypeId = null, string resourceId = null, [FromUri]string[] languages = null)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0] as string, language, parameters[1] as string),
                    IsValidRefData,
                    languages,
                    betTypeId,
                    resourceId ?? string.Empty);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> BetTypeNameWithRefNoAndResourceId(string betTypeId = null, string resourceId = null, long refNo = 0, [FromUri]string[] languages = null)
        {
            var betTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetBetTypeName(parameters[0] as string, language, parameters[1] as string, (long)parameters[2]),
                    IsValidRefData,
                    languages,
                    betTypeId,
                    resourceId ?? string.Empty,
                    refNo);

            return betTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> SportName(string sportTypeId = null, [FromUri]string[] languages = null)
        {
            var sportNames = GetRefResult(
                    (parameters, language) => _refData.GetSportName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    sportTypeId);

            return sportNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName(string teamId = null, [FromUri]string[] languages = null)
        {
            var teamNames = GetRefResult(
                    (parameters, language) => _refData.GetTeamName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    teamId);

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> TeamName3rd(string team3rdId = null, [FromUri]string[] languages = null)
        {
            var teamNames = GetRefResult(
                    (parameters, language) => _refData.GetTeamName3rd(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    team3rdId);

            return teamNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName(string leagueId = null, [FromUri]string[] languages = null)
        {
            var leagueNames = GetRefResult(
                    (parameters, language) => _refData.GetLeagueName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    leagueId);

            return leagueNames;
        }

        [HttpGet]
        public Dictionary<string, string> LeagueName3rd(string league3rdId = null, [FromUri]string[] languages = null)
        {
            var league3rdNames = GetRefResult(
                    (parameters, language) => _refData.GetLeagueName3rd(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    league3rdId);

            return league3rdNames;
        }

        [HttpGet]
        public Dictionary<string, string> ResourceData(string refId = null, string resourceId = null, [FromUri]string[] languages = null)
        {
            var resourceData = GetRefResult(
                    (parameters, language) => _refData.GetResourceDataByRefIdAndResourceId(parameters[0] as string, parameters[1] as string, language),
                    IsValidRefData,
                    languages,
                    refId,
                    resourceId);

            return resourceData;
        }

        [HttpGet]
        public Dictionary<string, string> CountryName(string countryCode = null, [FromUri]string[] languages = null)
        {
            var countryNames = GetRefResult(
                    (parameters, language) => _refData.GetCountryName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    countryCode);

            return countryNames;
        }

        [HttpGet]
        public Dictionary<string, string> OddsTypeName(string oddsTypeId = null, [FromUri]string[] languages = null)
        {
            var oddsTypeNames = GetRefResult(
                    (parameters, language) => _refData.GetOddsTypeName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    oddsTypeId);

            return oddsTypeNames;
        }

        [HttpGet]
        public Dictionary<string, string> ParentIdByBetTypeId(string betTypeId = null)
        {
            var parentId = GetRefResult(
                    (parameters, _) => _refData.GetParentIdByBetTypeId(parameters[0] as string),
                    IsValidRefData,
                    new[] { LanguageHelper.DefaultLanguage },
                    betTypeId);

            return parentId;
        }

        [HttpGet]
        public Dictionary<string, string[]> CombinationTypeComboParlay(
            string betTeam = null,
            string betCheck = null,
            int handicap1 = 0,
            int handicap2 = 0,
            [FromUri]string[] languages = null)
        {
            var combinationTypes = GetRefResult(
                    (parameters, language) => _refData.GetCombinationTypeCombo(parameters[0] as string, parameters[1] as string, (int)parameters[2], (int)parameters[3], language),
                    (refData) => refData.Any(item => !string.IsNullOrEmpty(item)),
                    languages,
                    betTeam,
                    betCheck,
                    handicap1,
                    handicap2);

            return combinationTypes;
        }

        [HttpGet]
        public Dictionary<string, string> HorseTeamName(string codeId = null, [FromUri]string[] languages = null)
        {
            var horseTeamNames = GetRefResult(
                    (parameters, language) => _refData.GetHorseTeamName(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    codeId);

            return horseTeamNames;
        }

        [HttpGet]
        public Dictionary<string, string> CasinoBetItem(string itemId = null, [FromUri]string[] languages = null)
        {
            var casinoBetItems = GetRefResult(
                    (parameters, language) => _refData.GetCasinoBetItem(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    itemId);

            return casinoBetItems;
        }

        [HttpGet]
        public Dictionary<string, string> RouletteBetItem(string itemId = null, [FromUri]string[] languages = null)
        {
            var rouletteBetItems = GetRefResult(
                    (parameters, language) => _refData.GetRouletteBetItem(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    itemId);

            return rouletteBetItems;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> KenoBetChoice(string kenoBetChoiceId = null, [FromUri]string[] languages = null)
        {
            Tuple<string, string> GetKenoBetChoice(object[] parameters, string language)
            {
                var kenoBetChoice = _refData.GetKenoBetChoice(parameters[0] as string, language, out string kenoBetTypeId);

                return new Tuple<string, string>(kenoBetChoice, kenoBetTypeId);
            }

            var kenoBetChoices = GetRefResult(
                    GetKenoBetChoice,
                    (refData) => !string.IsNullOrEmpty(refData.Item1),
                    languages,
                    kenoBetChoiceId);

            return kenoBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> KenoBetType(string kenoBetTypeId = null, [FromUri]string[] languages = null)
        {
            var kenoBetTypes = GetRefResult(
                    (parameters, language) => _refData.GetKenoBetType(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    kenoBetTypeId);

            return kenoBetTypes;
        }

        [HttpGet]
        public Dictionary<string, string> FastMarketsChoice(string betTypeId = null, string betTeam = null, int handicap1 = 0, int handicap2 = 0, [FromUri]string[] languages = null)
        {
            var fastMarketsChoices = GetRefResult(
                    (parameters, language) => _refData.GetFastMarketsChoice(
                        parameters[0] as string,
                        parameters[1] as string,
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
        public Dictionary<string, string> FastMarkets225Result(string resultId = null, [FromUri]string[] languages = null)
        {
            var fastMarkets225Results = GetRefResult(
                    (parameters, language) => _refData.GetFastMarkets225Result(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    resultId);

            return fastMarkets225Results;
        }

        [HttpGet]
        public Dictionary<string, Tuple<string, string>> LotteryBetChoice(string betChoiceId = null, [FromUri]string[] languages = null)
        {
            Tuple<string, string> GetLotteryBetChoice(object[] parameters, string language)
            {
                var lotteryBetChoice = _refData.GetLotteryBetChoice(parameters[0] as string, language, out string lotteryBetTypeID);

                return new Tuple<string, string>(lotteryBetChoice, lotteryBetTypeID);
            }

            var lotteryBetChoices = GetRefResult(
                    GetLotteryBetChoice,
                    (refData) => !string.IsNullOrEmpty(refData.Item1),
                    languages,
                    betChoiceId);

            return lotteryBetChoices;
        }

        [HttpGet]
        public Dictionary<string, string> LotteryBetType(string betTypeId = null, [FromUri]string[] languages = null)
        {
            var lotteryBetTypes = GetRefResult(
                    (parameters, language) => _refData.GetLotteryBetType(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    betTypeId);

            return lotteryBetTypes;
        }

        [HttpGet]
        public Dictionary<string, string> BetRadarMatchCodeFormat(string matchCode = null, [FromUri]string[] languages = null)
        {
            var refResult = GetRefResult(
                    (parameters, language) => _refData.GetBetRadarMatchCodeFormat(parameters[0] as string, language),
                    IsValidRefData,
                    languages,
                    matchCode);

            return refResult;
        }

        [HttpGet]
        public Dictionary<string, string> DisplayedTypeId(string betTypeId = null)
        {
            var refResult = GetRefResult(
                    (parameters, _) => _refData.GetDisplayedTypeId(parameters[0] as string),
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