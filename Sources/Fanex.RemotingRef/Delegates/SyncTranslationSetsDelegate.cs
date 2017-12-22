namespace Fanex.RemotingRef.Delegates
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TranslationStorage;

    public delegate Task<IDictionary<string, TranslationSet>> SyncTranslationSetsDelegate(IList<KeySet> parameterSets);
}