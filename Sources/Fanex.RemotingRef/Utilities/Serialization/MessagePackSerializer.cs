namespace Fanex.RemotingRef.Utilities.Serialization
{
    using System.IO;
    using MessagePack.Resolvers;

    public class MessagePackSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return MessagePack.MessagePackSerializer.Serialize(obj, ContractlessStandardResolver.Instance);
        }

        public T Deserialize<T>(byte[] obj)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(obj);
        }

        public T Deserialize<T>(Stream readStream)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(readStream);
        }
    }
}