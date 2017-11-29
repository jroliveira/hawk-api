namespace Hawk.WebApi.Lib.Middlewares
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.Response;

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var headers = response.Headers;
            const int OneYearInSeconds = 60 * 60 * 24 * 365;

            headers["X-Frame-Options"] = "deny";
            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Strict-Transport-Security"] = $"max-age={OneYearInSeconds}; includeSubDomains; preload";
            headers["X-Content-Type-Options"] = "nosniff";
            headers["Content-Security-Policy"] = "default-src 'none'";
            headers["Server"] = "hawk.com";

            await this.next(context);
        }
    }
}
