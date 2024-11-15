namespace Fanex.RemotingRef.Delegates;

using System.Threading.Tasks;
using Fanex.RemotingRef.TranslationStorage;

internal delegate Task<TranslationSet> RequestTranslationSetDelegate(KeySet parameters);