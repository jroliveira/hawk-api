namespace Hawk.WebApi.Infrastructure.Hal
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.WebApi.Infrastructure.Hal.Link;
    using Hawk.WebApi.Infrastructure.Hal.Page;
    using Hawk.WebApi.Infrastructure.Hal.Resource;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Formatters;

    using Newtonsoft.Json;

    using static System.Buffers.ArrayPool<char>;
    using static System.Text.Encoding;

    using static Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

    using static Newtonsoft.Json.JsonConvert;

    internal sealed class HalJsonOutputFormatter : JsonOutputFormatter
    {
        private const string ContentType = "application/hal+json";

        private readonly IReadOnlyDictionary<Type, Func<HttpContext, object, IResource>> builders;

        internal HalJsonOutputFormatter(IReadOnlyDictionary<Type, Func<HttpContext, object, IResource>> builders, Action<JsonSerializerSettings> setupAction)
            : base(JsonSettings.JsonSerializerSettings, Shared)
        {
            this.builders = builders;
            this.SerializerSettings.Converters.Add(new ResourceJsonConverter());
            this.SerializerSettings.Converters.Add(new LinksJsonConverter());
            this.SerializerSettings.Converters.Add(new PageJsonConverter());

            setupAction?.Invoke(this.SerializerSettings);

            this.SupportedEncodings.Add(UTF8);
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(Parse(ContentType));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var resource = this.builders[context.ObjectType](context.HttpContext, context.Object);
            var json = SerializeObject(resource, this.SerializerSettings);

            var response = context.HttpContext.Response;
            response.ContentType = ContentType;

            return response.WriteAsync(json);
        }
    }
}
