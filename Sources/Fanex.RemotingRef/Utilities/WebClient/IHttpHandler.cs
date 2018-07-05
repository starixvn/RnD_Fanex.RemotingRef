#pragma warning disable S3994 // URI Parameters should not be strings

namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpHandler
    {
        HttpResponseMessage Get(string url);

        HttpResponseMessage Post(string url, HttpContent content);

        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}