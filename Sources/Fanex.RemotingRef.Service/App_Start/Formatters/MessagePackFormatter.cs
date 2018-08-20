namespace Fanex.RemotingRef.Service.Formatters
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using MessagePack;
    using MessagePack.Resolvers;

    public class MessagePackFormatter : MediaTypeFormatter
    {
        public const string MimeType = "application/x-msgpack";

        public MessagePackFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MimeType));
        }

        public override bool CanReadType(Type type)
            => true;

        public override bool CanWriteType(Type type)
            => true;

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (writeStream == null)
            {
                throw new ArgumentNullException(nameof(writeStream));
            }

            MessagePackSerializer.NonGeneric.Serialize(type, writeStream, value, ContractlessStandardResolver.Instance);

            return Task.FromResult(0);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (readStream == null)
            {
                throw new ArgumentNullException(nameof(readStream));
            }

            var value = MessagePackSerializer.NonGeneric.Deserialize(type, readStream, ContractlessStandardResolver.Instance);

            return Task.FromResult(value);
        }
    }
}