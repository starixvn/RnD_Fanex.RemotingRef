namespace Fanex.RemotingRef.Utilities.Serialization
{
    using System.IO;
    using System.Threading.Tasks;
    using MessagePack;

    public class MessagePackSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
            => MessagePack.MessagePackSerializer.Serialize(obj, MessagePackSerializerOptions.Standard);

        public T Deserialize<T>(Stream readStream)
            => MessagePack.MessagePackSerializer.Deserialize<T>(readStream, MessagePackSerializerOptions.Standard);

        public async Task<T> DeserializeAsync<T>(Stream readStream)
            => await MessagePack.MessagePackSerializer.DeserializeAsync<T>(readStream, MessagePackSerializerOptions.Standard);
    }
}