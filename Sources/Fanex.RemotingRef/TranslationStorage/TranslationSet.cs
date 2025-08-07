#pragma warning disable S3925 // "ISerializable" should be implemented correctly

namespace Fanex.RemotingRef.TranslationStorage;

using System.Collections.Generic;

public sealed class TranslationSet : Dictionary<string, object>
{
    public TranslationSet(int capacity, IEqualityComparer<string> comparer)
        : base(capacity, comparer)
    {
    }

    public static TranslationSet FromDictionary<T>(IDictionary<string, T> dictionary)
    {
        if (dictionary == null)
        {
            return new TranslationSet(0, null);
        }

        var translationSet = new TranslationSet(dictionary.Count, null);

        foreach (var pair in dictionary)
        {
            translationSet.Add(pair.Key, pair.Value);
        }

        return translationSet;
    }
}