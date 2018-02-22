namespace Fanex.RemotingRef.Service.Models
{
    using System.Collections.Generic;

    public sealed class RequestSyncFormData
    {
        public IList<Dictionary<string, object>> ParameterSets { get; set; }

        public string[] Languages { get; set; }
    }
}