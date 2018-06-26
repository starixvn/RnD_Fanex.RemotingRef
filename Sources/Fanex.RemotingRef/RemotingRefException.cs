namespace Fanex.RemotingRef
{
    using System;
    using System.Collections.Generic;
    using System.Text;

#pragma warning disable S3925 // "ISerializable" should be implemented correctly

    public class RemotingRefException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        private readonly StringBuilder contentBuilder = new StringBuilder();

        public RemotingRefException()
        {
        }

        public RemotingRefException(string message)
            : base(message)
        {
        }

        public RemotingRefException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RemotingRefException(IDictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                contentBuilder.AppendLine($"\t{param.Key}: {param.Value}");
            }
        }

        public override string ToString() => $"{contentBuilder.ToString()} \r\n {base.ToString()}";
    }
}