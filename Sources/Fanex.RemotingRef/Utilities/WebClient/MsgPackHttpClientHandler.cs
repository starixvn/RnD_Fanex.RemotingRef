#pragma warning disable S3994 // URI Parameters should not be strings
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings

namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class MsgPackHttpClientHandler : IHttpHandler
    {
        private const string MimeType = "application/x-msgpack";
        private readonly HttpClient _client = new HttpClient();

        public MsgPackHttpClientHandler(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeType));
        }

        public HttpResponseMessage Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            return GetAsync(url).Result;
        }

        public HttpResponseMessage Post(string url, HttpContent content)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            return PostAsync(url, content).Result;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            return _client.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            return _client.PostAsync(url, content);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}