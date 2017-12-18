namespace Fanex.RemotingRef.Delegates
{
    using System.Collections.Generic;
    using TranslationStorage;

    public delegate IDictionary<string, TranslationSet> SyncDataDelegate(IList<KeySet> keySets);
}