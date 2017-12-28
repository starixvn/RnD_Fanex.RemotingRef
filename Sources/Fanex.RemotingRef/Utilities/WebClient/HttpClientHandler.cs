namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HttpClientHandler : IHttpHandler
    {
        private readonly Uri _baseAddress;

        public HttpClientHandler(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public HttpResponseMessage Get(Uri url)
        {
            return GetAsync(url).Result;
        }

        public HttpResponseMessage Post(Uri url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used

        public async Task<HttpResponseMessage> GetAsync(Uri url)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                return await client.GetAsync(url);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                return await client.PostAsync(url, content);
            }
        }

#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
    }
}