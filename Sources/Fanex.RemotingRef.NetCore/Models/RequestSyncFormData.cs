namespace Fanex.RemotingRef.NetCore.Models
{
    using System.Collections.Generic;
    using MessagePack;

    [MessagePackObject]
    public sealed class RequestSyncFormData
    {
        [Key(0)]
        public IList<ParameterSet> ParameterSets { get; set; }

        [Key(1)]
        public string[] Languages { get; set; }

    }
}