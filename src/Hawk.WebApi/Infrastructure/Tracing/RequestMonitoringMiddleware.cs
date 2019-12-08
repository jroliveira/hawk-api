namespace Hawk.WebApi.Infrastructure.Tracing
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using OpenTracing;

    using static OpenTracing.Tag.Tags;

    public sealed class RequestMonitoringMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ITracer tracer;

        public RequestMonitoringMiddleware(RequestDelegate next, ITracer tracer)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.tracer = tracer;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using var scope = this.tracer.BuildSpan("HttpRequest").StartActive();

            try
            {
                await this.next(httpContext);

                scope.Span
                    .SetTag(SpanKind, SpanKindServer)
                    .SetTag(HttpMethod, "GET")
                    .SetTag(HttpUrl, httpContext.Request.Path)
                    .SetTag(HttpStatus, httpContext.Response?.StatusCode ?? 0);
            }
            catch (Exception)
            {
                scope.Span.SetTag(Error, true);
                throw;
            }
        }
    }
}
