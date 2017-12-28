namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpHandler
    {
        HttpResponseMessage Get(Uri url);

        HttpResponseMessage Post(Uri url, HttpContent content);

        Task<HttpResponseMessage> GetAsync(Uri url);

        Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content);
    }
}