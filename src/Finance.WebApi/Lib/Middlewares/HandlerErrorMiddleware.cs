namespace Finance.WebApi.Lib.Middlewares
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Finance.Infrastructure.Exceptions;
    using Finance.WebApi.Models;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    public class HandlerErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDictionary<string, int> statusCode;

        public HandlerErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
            this.statusCode = new Dictionary<string, int>
            {
                { "ValidationException", 409 },
                { "NotFoundException", 404 }
            };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
                if (context.Response.StatusCode == 404)
                {
                    throw new NotFoundException($"Resource '{context.Request.Path.Value}' could not be found");
                }
            }
            catch (Exception exception)
            {
                var model = new Error
                {
                    Message = $"Exception: {exception.Message}. Inner exception: {exception.InnerException?.Message}"
                };

                this.statusCode.TryGetValue(exception.GetType().Name, out int code);
                context.Response.StatusCode = code != 0 ? code : 500;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(model));
            }
        }
    }
}