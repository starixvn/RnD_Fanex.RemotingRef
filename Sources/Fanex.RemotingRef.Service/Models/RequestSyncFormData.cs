namespace Fanex.RemotingRef.Service.Models
{
    using System.Collections.Generic;

    public sealed class RequestSyncFormData
    {
        public IList<ParameterSet> ParameterSets { get; set; }

        public string[] Languages { get; set; }

        public string ApplicationName { get; set; }
    }
}