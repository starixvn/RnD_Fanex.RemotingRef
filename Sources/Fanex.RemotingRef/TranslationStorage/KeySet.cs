namespace Fanex.RemotingRef.TranslationStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

    internal sealed class KeySet : Dictionary<string, object>
    {
        public string BuildKey()
            => this.Aggregate(
                new StringBuilder(),
                (keyBuilder, pair) => keyBuilder.Append("{").Append(pair.Key).Append(":").Append(pair.Value).Append("}"),
                keyBuilder => keyBuilder.ToString());
    }
}