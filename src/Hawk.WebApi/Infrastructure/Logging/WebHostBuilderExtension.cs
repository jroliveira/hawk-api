namespace Hawk.WebApi.Infrastructure.Logging
{
    using System;

    using Hawk.Infrastructure.Logging.Configurations;
    using Hawk.Infrastructure.Logging.Methods;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Extensions.Logging;

    using static System.Enum;

    using static Hawk.Infrastructure.Logging.Logger;

    using ILogger = Serilog.ILogger;
    using LogLevel = Hawk.Infrastructure.Logging.LogLevel;

    internal static class WebHostBuilderExtension
    {
        internal static IWebHostBuilder ConfigureLogging(this IWebHostBuilder @this)
        {
            ILogger? logger = default;

            return @this
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
                        () => default,
                        new SerilogLogMethod(logConfig, value => { logger = value; }).Write);
                })
                .ConfigureLogging((_, logging) => logging.AddSerilog(logger))
                .ConfigureServices((_, service) => service.AddSingleton<ILoggerFactory>(provider => new SerilogLoggerFactory(logger)));
        }
    }
}
