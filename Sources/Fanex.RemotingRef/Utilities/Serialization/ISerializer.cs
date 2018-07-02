namespace Fanex.RemotingRef.Utilities.Serialization
{
    using System.IO;
    using System.Threading.Tasks;

    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(Stream readStream);

        Task<T> DeserializeAsync<T>(Stream readStream);
    }
}