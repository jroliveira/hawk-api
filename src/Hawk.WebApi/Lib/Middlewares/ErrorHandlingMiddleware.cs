namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Threading.Tasks;

    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    using static Hawk.Infrastructure.Guard;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Newtonsoft.Json.JsonConvert;

    internal sealed class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            NotNull(next, nameof(next), "Error handling middleware's next cannot be null.");

            this.next = next;
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
                await context.Response.WriteAsync(SerializeObject(new Error(exception.Message)));
            }
        }
    }
}
