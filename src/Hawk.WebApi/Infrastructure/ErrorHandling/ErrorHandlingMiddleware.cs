namespace Hawk.WebApi.Infrastructure.ErrorHandling
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.WebApi.Infrastructure.Hal;

    using Microsoft.AspNetCore.Http;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Serialization.JsonSettings;
    using static Hawk.WebApi.Infrastructure.Hal.ResourceBuilders;

    using static Newtonsoft.Json.JsonConvert;

    internal sealed class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);

                switch (context.Response.StatusCode)
                {
                    case 404:
                        await WriteResponse(context, new NotFoundException($"The HTTP resource that matches the request URI '{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.Path.Value}{context.Request.QueryString.Value}' not found."), 404);
                        break;
                }
            }
            catch (Exception exception)
            {
                await WriteResponse(context, exception, 500);
            }
        }

        private static async Task WriteResponse(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var serializerSettings = JsonSerializerSettings;
            serializerSettings.AddHal();

            LogError("An unhandled error has occurred.", exception);

            var errorModel = HandleException(exception);

            await context.Response.WriteAsync(SerializeObject(
                GetResource(context, errorModel, errorModel.GetType()),
                serializerSettings));
        }
    }
}
