namespace Fanex.RemotingRef.NetCore.Models
{
    using System.Collections.Generic;
    using MessagePack;

    public sealed class RequestSyncFormData
    {
        public IList<ParameterSet> ParameterSets { get; set; }

        public string[] Languages { get; set; }

    }

[MessagePackObject]
     public class RequestSyncFormData1
    {
        [Key(0)]
        public string Languages { get; set; }
    }
}