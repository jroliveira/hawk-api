namespace Hawk.WebApi.Lib.Middlewares
{
    using System.Threading.Tasks;

    using Hawk.Infrastructure;

    using Microsoft.AspNetCore.Http;

    internal sealed class SecurityHeadersMiddleware
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 365;
        private readonly RequestDelegate next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            Guard.NotNull(next, nameof(next), "Security headers middleware's next cannot be null.");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Guard.NotNull(context, nameof(context), "SecurityHeadersMiddleware context cannot be null.");
            Guard.NotNull(context.Response, nameof(context.Response), "SecurityHeadersMiddleware response cannot be null.");

            context.Response.Headers["X-Frame-Options"] = "deny";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["Strict-Transport-Security"] = $"max-age={OneYearInSeconds}; includeSubDomains; preload";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["Content-Security-Policy"] = "default-src 'none'";
            context.Response.Headers["X-Content-Security-Policy"] = "default-src 'none'";
            context.Response.Headers["Server"] = "hawk.com";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";

            await this.next(context);
        }
    }
}
