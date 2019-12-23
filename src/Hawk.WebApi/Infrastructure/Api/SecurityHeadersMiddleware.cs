namespace Hawk.WebApi.Infrastructure.Api
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using static Hawk.Infrastructure.Guard;

    internal sealed class SecurityHeadersMiddleware
    {
        private const int OneYearInSeconds = 60 * 60 * 24 * 365;
        private readonly RequestDelegate next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            NotNull(next, nameof(next), "Security headers middleware's next cannot be default.");

            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            NotNull(context, nameof(context), "SecurityHeadersMiddleware context cannot be default.");
            NotNull(context.Response, nameof(context.Response), "SecurityHeadersMiddleware response cannot be default.");

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
