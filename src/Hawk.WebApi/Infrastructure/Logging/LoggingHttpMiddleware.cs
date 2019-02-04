namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using static Hawk.Infrastructure.Guard;
    using static Hawk.Infrastructure.Logging.Logger;

    internal class LoggingHttpMiddleware
    {
        private readonly RequestDelegate next;

        public LoggingHttpMiddleware(RequestDelegate next)
        {
            NotNull(next, nameof(next), "Http log middleware's next cannot be null.");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await this.next(context);

            stopwatch.Stop();

            Info(new LogData("Time spent during request execution.", stopwatch, context));
        }

        internal sealed class LogData
        {
            internal LogData(string message, Stopwatch stopwatch, HttpContext context)
            {
                this.Message = message;
                this.Elapse = new Elapse(stopwatch);
                this.Request = new Request(context);
                this.Response = new Response(context);
            }

            internal string Message { get; }

            internal Elapse Elapse { get; }

            internal Request Request { get; }

            internal Response Response { get; }
        }

        internal class Elapse
        {
            internal Elapse(Stopwatch stopwatch)
            {
                this.TimeSpan = stopwatch.Elapsed;
                this.Milliseconds = stopwatch.Elapsed.Milliseconds;
            }

            internal TimeSpan TimeSpan { get; }

            internal int Milliseconds { get; }
        }

        internal class Request
        {
            internal Request(HttpContext context)
            {
                this.ContentType = context.Request.ContentType;
                this.Headers = context.Request.Headers;
                this.Method = context.Request.Method;
            }

            internal string ContentType { get; }

            internal IHeaderDictionary Headers { get; }

            internal string Method { get; }
        }

        internal class Response
        {
            internal Response(HttpContext context)
            {
                this.ContentType = context.Response.ContentType;
                this.Headers = context.Response.Headers;
                this.StatusCode = context.Response.StatusCode;
            }

            internal string ContentType { get; }

            internal IHeaderDictionary Headers { get; }

            internal int StatusCode { get; }
        }
    }
}
