namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;

    public interface IDataTypeHandler
    {
        TimeSpan SlidingTimeout { get; }

        TimeSpan RefreshInterval { get; }

        Func<string, string> ValueFactory { get; }

        Func<IList<string>, IDictionary<string, string>> UpdateByBatchHanlder { get; }

        string Get(string key);
    }
}