namespace Fanex.RemotingRef.Storage
{
    using System.Collections.Generic;

    internal interface IStorage
    {
        bool TryGet(
            string key,
            out IDictionary<string, string> result,
            out bool needToRenew);

        void Set(string key, IDictionary<string, string> value);

        IList<CacheEntry> FilterOutOfDateEntries();
    }
}