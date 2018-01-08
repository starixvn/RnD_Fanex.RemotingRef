namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used
#pragma warning disable S3994 // URI Parameters should not be strings
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings

    public class HttpClientHandler : IHttpHandler
    {
        private readonly Uri _baseAddress;

        public HttpClientHandler(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public HttpResponseMessage Get(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            return GetAsync(url).Result;
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                return await client.GetAsync(url);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            using (var client = new HttpClient { BaseAddress = _baseAddress })
            {
                return await client.PostAsync(url, content);
            }
        }
    }

#pragma warning restore S4005 // "System.Uri" arguments should be used instead of strings
#pragma warning restore S3994 // URI Parameters should not be strings
#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
}