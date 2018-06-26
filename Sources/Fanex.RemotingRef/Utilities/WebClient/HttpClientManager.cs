namespace Fanex.RemotingRef.Utilities.WebClient
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    internal static class HttpClientManager
    {
        private static readonly object SyncRoot = new object();
        private static HttpClient client;
        internal const string MimeType = "application/x-msgpack";

        internal static HttpClient CreateHttpClient()
        {
            if (client == null)
            {
                lock (SyncRoot)
                {
                    if (client == null)
                    {
                        client = new HttpClient
                        {
                            BaseAddress = RefDataTypeContainer.ServiceUri
                        };

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeType));
                    }
                }
            }

            return client;
        }
    }
}