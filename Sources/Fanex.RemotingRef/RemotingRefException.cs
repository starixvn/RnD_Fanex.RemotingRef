namespace Fanex.RemotingRef
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;

    [Serializable]
    public class RemotingRefException : Exception
    {
        public RemotingRefException(Uri requestUri, HttpStatusCode statusCode, string failedReason)
            : base(BuildMessage(requestUri, statusCode, failedReason))
        {
            RequestUri = requestUri;
            StatusCode = statusCode;
            FailedReason = failedReason;
        }

        protected RemotingRefException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RequestUri = info.GetValue(nameof(RequestUri), typeof(Uri)) as Uri;
            StatusCode = (HttpStatusCode)info.GetInt32(nameof(StatusCode));
            FailedReason = info.GetString(nameof(FailedReason));
        }

        public Uri RequestUri { get; }

        public HttpStatusCode StatusCode { get; }

        public string FailedReason { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(RequestUri), RequestUri);
            info.AddValue(nameof(StatusCode), StatusCode);
            info.AddValue(nameof(FailedReason), FailedReason);

            base.GetObjectData(info, context);
        }

        private static string BuildMessage(Uri requestUri, HttpStatusCode statusCode, string failedReason)
            => new StringBuilder()
                .AppendLine("Failed to send request to Fanex.RemotingRef.Service:")
                .AppendLine($"\tRequest URL: '{requestUri}'")
                .AppendLine($"\tResponse's Status Code: {statusCode}")
                .AppendLine($"\tReason: {failedReason}")
                .ToString();
    }
}