namespace Fanex.RemotingRef.Utilities.Serialization
{
    using System.IO;

    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] obj);

        T Deserialize<T>(Stream readStream);
    }
}