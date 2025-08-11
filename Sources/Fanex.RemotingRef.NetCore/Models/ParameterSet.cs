namespace Fanex.RemotingRef.NetCore.Models
{
    using System.Collections.Generic;
    using MessagePack;

    [MessagePackObject]
    public class ParameterSet : Dictionary<string, object>
    {
    }
}