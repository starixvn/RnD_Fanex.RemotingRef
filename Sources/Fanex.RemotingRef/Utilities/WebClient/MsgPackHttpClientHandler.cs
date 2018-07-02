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
        internal const string MimeType = "application/x-msgpack";

        private static readonly object _syncRoot = new object();
        private static HttpClient _client;

        internal static HttpClient Client => CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            if (_client == null)
            {
                lock (_syncRoot)
                {
                    if (_client == null)
                    {
                        _client = new HttpClient
                        {
                            BaseAddress = RefDataTypeContainer.ServiceUri
                        };

                        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeType));
                    }
                }
            }

            return _client;
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

            return Client.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);

            return Client.PostAsync(url, content);
        }
    }
}