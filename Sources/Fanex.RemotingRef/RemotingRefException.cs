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
        public RemotingRefException(string request, HttpStatusCode statusCode, string failedReason)
            : base(BuildMessage(request, statusCode, failedReason))
        {
            Request = request;
            StatusCode = statusCode;
            FailedReason = failedReason;
        }

        protected RemotingRefException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Request = info.GetString(nameof(Request));
            StatusCode = (HttpStatusCode)info.GetInt32(nameof(StatusCode));
            FailedReason = info.GetString(nameof(FailedReason));
        }

        public string Request { get; }

        public HttpStatusCode StatusCode { get; }

        public string FailedReason { get; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Request), Request);
            info.AddValue(nameof(StatusCode), StatusCode);
            info.AddValue(nameof(FailedReason), FailedReason);

            base.GetObjectData(info, context);
        }

        private static string BuildMessage(string request, HttpStatusCode statusCode, string failedReason)
            => new StringBuilder()
                .AppendLine("Failed to send request to Fanex.RemotingRef.Service:")
                .AppendLine($"\tRequest URL: '{request}'")
                .AppendLine($"\tResponse's Status Code: {statusCode}")
                .AppendLine($"\tReason: {failedReason}")
                .ToString();
    }
}