namespace Fanex.RemotingRef.Service.Utilities.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] obj);
    }
}