namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpHandler : IDisposable
    {
#pragma warning disable S3994 // URI Parameters should not be strings

        HttpResponseMessage Get(string url);

        HttpResponseMessage Post(string url, HttpContent content);

        Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);

#pragma warning restore S3994 // URI Parameters should not be strings
    }
}