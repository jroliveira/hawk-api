namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Logging.Configurations;
    using Hawk.Infrastructure.Logging.Methods;
    using Hawk.WebApi.Infrastructure.Logging.Http;

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

            var logConfig = configuration
                .GetSection("log")
                .Get<LogConfiguration>();

            if (!TryParse(logConfig.Level, out LogLevel level))
            {
                throw new InvalidCastException($"LogLevel {logConfig.Level} is not valid.");
            }

            NewLogger(
                level,
                () => accessor.HttpContext.Request.Headers["reqId"],
                new SerilogLogMethod(logConfig).Write);

            return @this;
        }
    }
}
