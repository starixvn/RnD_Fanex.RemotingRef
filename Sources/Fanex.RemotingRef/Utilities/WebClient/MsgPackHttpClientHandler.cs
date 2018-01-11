namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

#pragma warning disable S3216 // "ConfigureAwait(false)" should be used
#pragma warning disable S3994 // URI Parameters should not be strings
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings

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
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);

            return await _client.PostAsync(url, content);
        }
    }

#pragma warning restore S4005 // "System.Uri" arguments should be used instead of strings
#pragma warning restore S3994 // URI Parameters should not be strings
#pragma warning restore S3216 // "ConfigureAwait(false)" should be used
}