namespace Fanex.RemotingRef.TranslationStorage
{
    using System.Collections.Generic;

    public interface IStorage
    {
        ISystemClock Clock { get; }

        CacheEntry CreateEntry(string key, TranslationSet value, ExtraDataSet extraData);

        void Add(string key, CacheEntry entry);

        bool TryGet(string key, out CacheEntry result);

        void TryUpdate(CacheEntry entry, CacheEntry previousEntry);

        IList<CacheEntry> FilterOutOfDateEntries(int batchSize);
    }
}