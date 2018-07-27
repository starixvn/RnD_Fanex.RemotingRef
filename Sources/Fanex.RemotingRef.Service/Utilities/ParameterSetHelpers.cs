namespace Fanex.RemotingRef.Service.Utilities
{
    using Fanex.RemotingRef.Service.Models;

    internal static class ParameterSetHelpers
    {
        public static string GetBetTypeId(this ParameterSet parameterSet)
        {
            const string parameterName = "betTypeId";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName] as string : null;
        }

        public static string GetMatchCode(this ParameterSet parameterSet)
        {
            const string parameterName = "matchCode";

            return parameterSet.ContainsKey(parameterName) ? parameterSet[parameterName] as string : null;
        }
    }
}