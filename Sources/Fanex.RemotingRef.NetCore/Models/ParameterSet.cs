#pragma warning disable S3925 // "ISerializable" should be implemented correctly

namespace Fanex.RemotingRef.NetCore.Models
{
    using System.Collections.Generic;

    public class ParameterSet : Dictionary<string, object>
    {
    }
}