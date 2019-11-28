namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using System;
    using System.Threading.Tasks;

    using Hawk.WebApi.Infrastructure.ErrorHandling.TryModel;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using static ErrorHandler;

    using static Hawk.Infrastructure.JsonSettings;
    using static Hawk.Infrastructure.Logging.Logger;

    using static Newtonsoft.Json.JsonConvert;

    internal sealed class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment environment;

        public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            this.environment = environment;
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

                var serializerSettings = JsonSerializerSettings;
                serializerSettings.Converters.Add(new TryModelJsonConverter());

                await context.Response.WriteAsync(SerializeObject(HandleError<Unit>(exception, this.environment), serializerSettings));
            }
        }

        public sealed class Unit
        {
        }
    }
}
