namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Logging.Methods;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    using static System.Enum;

    using static Hawk.Infrastructure.Logging.Logger;

    internal static class ApplicationBuilderExtension
    {
        internal static IApplicationBuilder UseLogging(
            this IApplicationBuilder @this,
            IConfiguration configuration,
            IHttpContextAccessor accessor)
        {
            @this.UseMiddleware<LoggingHttpMiddleware>();

            if (!TryParse(configuration["log:level"], out LogLevel level))
            {
                throw new InvalidCastException($"LogLevel {configuration["log:level"]} is not valid.");
            }

            Action<string> logMethod = new DefaultLogMethod(configuration["log:file"]).Write;

            Init(level, () => accessor.HttpContext.Request.Headers["reqId"], logMethod);

            return @this;
        }
    }
}
