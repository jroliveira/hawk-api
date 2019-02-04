namespace Hawk.WebApi.Infrastructure.Api
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Hawk.WebApi.Features.Shared.ErrorModels;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using static Hawk.Infrastructure.Guard;
    using static Hawk.Infrastructure.Logging.Logger;

    using static Newtonsoft.Json.JsonConvert;

    internal sealed class ErrorHandlingMiddleware
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            Culture = CultureInfo.InvariantCulture,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
        };

        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;

        public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            NotNull(next, nameof(next), "Error handling middleware's next cannot be null.");
            NotNull(env, nameof(env), "Error handling middleware's env cannot be null.");

            this.next = next;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exception)
            {
                LogError("Unexpected error", exception);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(SerializeObject(new GenericErrorModel(exception, this.env), JsonSerializerSettings));
            }
        }
    }
}
