using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace MessagePack.AspNetCoreMvcFormatter
{
    public class MessagePackInputFormatter1 : InputFormatter
    {
        private const string ContentType = "application/x-msgpack";
        private readonly MessagePackSerializerOptions? options;

        public MessagePackInputFormatter1()
            : this(null)
        {
        }

        public MessagePackInputFormatter1(MessagePackSerializerOptions? options)
        {
            this.options = options;

            SupportedMediaTypes.Add(ContentType);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var result = await MessagePackSerializer.DeserializeAsync(context.ModelType, request.Body, this.options, context.HttpContext.RequestAborted).ConfigureAwait(false);
            return await InputFormatterResult.SuccessAsync(result).ConfigureAwait(false);
        }
    }
}