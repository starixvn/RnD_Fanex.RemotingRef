namespace Fanex.RemotingRef.TranslationStorage;

using System.Collections.Generic;

internal interface IStorage
{
    ISystemClock Clock { get; }

    CacheEntry CreateEntry(string key, KeySet keySet, TranslationSet value);

    void Add(CacheEntry entry);

    bool TryGet(string key, out CacheEntry result);

    IList<CacheEntry> FilterOutOfDateEntries(int batchSize);

    bool IsExpired(CacheEntry entry);
}