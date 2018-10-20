namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Hawk.Infrastructure;
    using Hawk.Infrastructure.Logging;

    using Microsoft.AspNetCore.Http;

    internal class HttpLogMiddleware
    {
        private readonly RequestDelegate next;

        public HttpLogMiddleware(RequestDelegate next)
        {
            Guard.NotNull(next, nameof(next), "Http log middleware's next cannot be null.");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await this.next(context);

            stopwatch.Stop();

            Logger.Info(new LogData("Time spent during request execution.", stopwatch, context));
        }

        internal sealed class LogData
        {
            public LogData(string message, Stopwatch stopwatch, HttpContext context)
            {
                this.Message = message;
                this.Elapse = new Elapse(stopwatch);
                this.Request = new Request(context);
                this.Response = new Response(context);
            }

            public string Message { get; }

            public Elapse Elapse { get; }

            public Request Request { get; }

            public Response Response { get; }
        }

        internal class Elapse
        {
            public Elapse(Stopwatch stopwatch)
            {
                this.TimeSpan = stopwatch.Elapsed;
                this.Milliseconds = stopwatch.Elapsed.Milliseconds;
            }

            public TimeSpan TimeSpan { get; }

            public int Milliseconds { get; }
        }

        internal class Request
        {
            public Request(HttpContext context)
            {
                this.ContentType = context.Request.ContentType;
                this.Headers = context.Request.Headers;
                this.Method = context.Request.Method;
            }

            public string ContentType { get; }

            public IHeaderDictionary Headers { get; }

            public string Method { get; }
        }

        internal class Response
        {
            public Response(HttpContext context)
            {
                this.ContentType = context.Response.ContentType;
                this.Headers = context.Response.Headers;
                this.StatusCode = context.Response.StatusCode;
            }

            public string ContentType { get; }

            public IHeaderDictionary Headers { get; }

            public int StatusCode { get; }
        }
    }
}
