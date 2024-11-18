namespace Fanex.RemotingRef.Delegates;

using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.RemotingRef.TranslationStorage;

internal delegate Task<IDictionary<string, TranslationSet>> SyncTranslationSetsDelegate(IList<KeySet> parameterSets);