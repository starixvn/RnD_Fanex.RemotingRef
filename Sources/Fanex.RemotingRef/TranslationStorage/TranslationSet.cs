namespace Fanex.RemotingRef.TranslationStorage
{
    using System;
    using System.Collections.Generic;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

    public sealed class TranslationSet : Dictionary<string, object>
    {
        private TranslationSet(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        {
        }

        internal static TranslationSet FromDictionary<T>(IDictionary<string, T> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            var translationSet = new TranslationSet(dictionary.Count, null);

            foreach (KeyValuePair<string, T> pair in dictionary)
            {
                translationSet.Add(pair.Key, pair.Value);
            }

            return translationSet;
        }
    }
}