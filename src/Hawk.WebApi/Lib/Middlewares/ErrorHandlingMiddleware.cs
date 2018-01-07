namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Hawk.Domain;
    using Hawk.Domain.Exceptions;
    using Hawk.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    internal sealed class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IReadOnlyDictionary<string, int> statusCode;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
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

                this.statusCode.TryGetValue(exception.GetType().Name, out var code);
                context.Response.StatusCode = code != 0 ? code : 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(model));
            }
        }
    }
}