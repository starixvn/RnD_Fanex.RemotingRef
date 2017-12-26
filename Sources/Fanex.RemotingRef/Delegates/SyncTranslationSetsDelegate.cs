namespace Fanex.RemotingRef.Delegates
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TranslationStorage;

    internal delegate Task<IDictionary<string, TranslationSet>> SyncTranslationSetsDelegate(IList<KeySet> parameterSets);
}