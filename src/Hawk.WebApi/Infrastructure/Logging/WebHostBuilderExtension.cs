namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Logging.Configurations;
    using Hawk.Infrastructure.Logging.Methods;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using Serilog;

    using static System.Enum;

    using static Hawk.Infrastructure.Logging.Logger;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureLogging(this IWebHostBuilder @this) => @this
            .SuppressStatusMessages(true)
            .UseSerilog((hostingContext, _) =>
            {
                var logConfig = hostingContext.Configuration
                    .GetSection("log")
                    .Get<LogConfiguration>();

                if (!TryParse(logConfig.Level, out LogLevel level))
                {
                    throw new InvalidCastException($"Log level '{logConfig.Level}' is not valid.");
                }

                var logMethod = new SerilogLogMethod(logConfig);

                NewLogger(
                    level,
                    (logLevel, data) => logMethod.Write(logLevel, data));
            });
    }
}
