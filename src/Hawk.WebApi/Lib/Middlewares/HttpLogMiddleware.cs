namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Logging;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    internal class HttpLogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Logger logger;

        public HttpLogMiddleware(RequestDelegate next, Logger logger)
        {
            Guard.NotNull(next, nameof(next), "HttpLogMiddleware next cannot be null.");
            Guard.NotNull(logger, nameof(logger), "HttpLogMiddleware logger cannot be null.");

            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var tracking = Guid.NewGuid().ToString();

            context.Request.Headers.Add("reqId", new StringValues(tracking));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await this.next(context);

            stopwatch.Stop();

            this.logger.Info(new DefaultLogData(
                this.logger.Level,
                tracking,
                new
                {
                    Message = "Time spent during request execution.",
                    Elapsed = new
                    {
                        TimeSpan = stopwatch.Elapsed,
                        stopwatch.Elapsed.Milliseconds
                    },
                    Request = new
                    {
                        context.Request.ContentType,
                        context.Request.Headers,
                        context.Request.Method
                    },
                    Response = new
                    {
                        context.Request.ContentType,
                        context.Response.Headers,
                        context.Response.StatusCode
                    }
                }));
        }
    }
}