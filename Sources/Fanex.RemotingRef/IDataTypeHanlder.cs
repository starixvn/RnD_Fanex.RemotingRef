namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;

    public interface IDataTypeHanlder
    {
        TimeSpan TimeToLive { get; }

        TimeSpan TimeToUpdate { get; }

        Func<string, string> SingleValueGetter { get; }

        Func<string[], IDictionary<string, string>> BatchOfValuesGetter { get; }

        IStorage Storage { get; }

        string Get(string key);
    }
}