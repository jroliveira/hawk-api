namespace Hawk.WebApi.Configuration
{
    using Hawk.WebApi.Lib.Middlewares;

    using Microsoft.AspNetCore.Builder;

    internal static class SecurityHeaders
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}