namespace Fanex.RemotingRef.Service.Utilities.Serialization
{
    public class MessagePackSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return MessagePack.MessagePackSerializer.Serialize(obj);
        }

        public T Deserialize<T>(byte[] obj)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(obj);
        }
    }
}