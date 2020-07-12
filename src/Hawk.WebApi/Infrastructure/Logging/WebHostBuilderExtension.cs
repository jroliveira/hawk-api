namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Logging.Configurations;
    using Hawk.Infrastructure.Logging.Methods;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using Serilog;

    using static System.Enum;

    using static Hawk.Infrastructure.Logging.Logger;

    using LogLevel = Hawk.Infrastructure.Logging.LogLevel;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureLogging(this IWebHostBuilder @this) => @this
            .UseSerilog((hostingContext, _) =>
            {
                var logConfig = hostingContext.Configuration
                    .GetSection("log")
                    .Get<LogConfiguration>();

                if (!TryParse(logConfig.Level, out LogLevel level))
                {
                    throw new InvalidCastException($"Log level '{logConfig.Level}' is not valid.");
                }

                NewLogger(
                    level,
                    (logLevel, data) => new SerilogLogMethod(logConfig).Write(logLevel, data));
            });
    }
}
