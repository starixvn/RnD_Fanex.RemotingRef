#pragma warning disable S3994 // URI Parameters should not be strings
#pragma warning disable S4005 // "System.Uri" arguments should be used instead of strings

namespace Fanex.RemotingRef.Utilities.WebClient;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

internal class MsgPackHttpClientHandler : IHttpHandler
{
    public const string MimeType = "application/x-msgpack";

    private static readonly object SyncRoot = new();
    private static HttpClient _client;

    public static HttpClient Client => GetHttpClient();

    private static HttpClient GetHttpClient()
    {
        if (_client == null)
        {
            lock (SyncRoot)
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

    public Task<HttpResponseMessage> GetAsync(string url)
        => Client.GetAsync(url);

    public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
    {
        content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);

        return Client.PostAsync(url, content);
    }
}