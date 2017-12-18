namespace Fanex.RemotingRef.TranslationStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

    public class KeySet : Dictionary<string, object>
    {
        public string ToHashKey()
            => Values.Aggregate(
                new StringBuilder(),
                (hashBuilder, value) => hashBuilder.Append("{").Append(value).Append("}"),
                hashBuilder => hashBuilder.ToString());
    }
}