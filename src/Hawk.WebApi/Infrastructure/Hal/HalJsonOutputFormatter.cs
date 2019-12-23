namespace Hawk.WebApi.Infrastructure.Hal
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;

    using Newtonsoft.Json;

    using static System.Text.Encoding;

    using static Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

    using static Newtonsoft.Json.JsonConvert;

    using static ResourceBuilders;

    internal sealed class HalJsonOutputFormatter : TextOutputFormatter
    {
        private const string ContentType = "application/hal+json";

        private readonly JsonSerializerSettings serializerSettings;

        internal HalJsonOutputFormatter(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
            this.serializerSettings.AddHal();

            this.SupportedEncodings.Add(UTF8);
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(Parse(ContentType));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            context.HttpContext.Response.ContentType = ContentType;

            return context.HttpContext.Response.WriteAsync(SerializeObject(
                GetResource(context.HttpContext, context.Object, context.ObjectType),
                this.serializerSettings));
        }
    }
}
