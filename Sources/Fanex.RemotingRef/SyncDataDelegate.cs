namespace Fanex.RemotingRef
{
    using System.Collections.Generic;
    using TranslationStorage;

    public delegate IDictionary<string, TranslationSet> SyncDataDelegate(IList<ExtraDataSet> parameterSets);
}