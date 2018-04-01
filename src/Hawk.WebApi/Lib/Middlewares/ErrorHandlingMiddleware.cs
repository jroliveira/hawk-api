namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Hawk.Domain;
    using Hawk.Infrastructure.Logging;
    using Hawk.WebApi.Lib.Exceptions;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    internal sealed class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        private readonly Logger logger;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IReadOnlyDictionary<string, int> statusCode;

        public ErrorHandlingMiddleware(RequestDelegate next, Logger logger, IHttpContextAccessor contextAccessor)
        {
            this.next = next;
            this.logger = logger;
            this.contextAccessor = contextAccessor;

            this.statusCode = new Dictionary<string, int>
            {
                { Constants.Exceptions.NotFound, 404 },
                { Constants.Exceptions.UnauthorizedAccess, 401 },
                { Constants.Exceptions.Validation, 409 }
            };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);

                switch (context.Response.StatusCode)
                {
                    case 404: throw new NotFoundException($"Resource '{context.Request.Path.Value}' could not be found");
                    case 401: throw new UnauthorizedAccessException($"Resource '{context.Request.Path.Value}' requires authentication");
                }
            }
            catch (Exception exception)
            {
                var message = new StringBuilder();
                message.Append($"Exception: {exception.Message}.");

                var innerException = exception.InnerException;

                while (innerException != null)
                {
                    message.Append($" Inner exception: {innerException.Message}");
                    innerException = innerException.InnerException;
                }

                var model = new Error(message.ToString());
                this.logger.Error(new DefaultLogData(this.logger.Level, context.Request.Headers["reqId"], model));

                this.statusCode.TryGetValue(exception.GetType().Name, out var code);
                context.Response.StatusCode = code != 0 ? code : 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(model));
            }
        }
    }
}