namespace Fanex.RemotingRef.Utilities.Serialization
{
    using System.IO;
    using System.Threading.Tasks;
    using MessagePack.Resolvers;

    public class MessagePackSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
            => MessagePack.MessagePackSerializer.Serialize(obj, ContractlessStandardResolver.Instance);

        public T Deserialize<T>(Stream readStream)
            => MessagePack.MessagePackSerializer.Deserialize<T>(readStream, ContractlessStandardResolver.Instance);

        public Task<T> DeserializeAsync<T>(Stream readStream)
            => MessagePack.MessagePackSerializer.DeserializeAsync<T>(readStream, ContractlessStandardResolver.Instance);
    }
}