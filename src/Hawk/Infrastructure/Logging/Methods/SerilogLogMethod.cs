namespace Hawk.Infrastructure.Logging.Methods
{
    using System;
    using System.Collections.Generic;

    using Hawk.Infrastructure.Logging.Configurations;
    using Hawk.Infrastructure.Tracing.SerilogSink;

    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.Elasticsearch;

    using static Serilog.Events.LogEventLevel;
    using static Serilog.RollingInterval;

    public sealed class SerilogLogMethod : ILogMethod
    {
        private static readonly IReadOnlyDictionary<LogLevel, LogEventLevel> Levels = new Dictionary<LogLevel, LogEventLevel>
        {
            { LogLevel.Error, Error },
            { LogLevel.Warn, Warning },
            { LogLevel.Info, Information },
            { LogLevel.Verb,  Verbose },
        };

        private readonly ILogger logger;

        public SerilogLogMethod(LogConfiguration config)
        {
            var loggerConfig = new LoggerConfiguration();
            var (_, (console, file, elasticSearch, tracing)) = config;

            if (console.Enabled != null && console.Enabled.Value)
            {
                loggerConfig.WriteTo.Console(
                    outputTemplate: "{NewLine}{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ---{NewLine}{Message:lj}{NewLine}");
            }

            if (file.Enabled != null && file.Enabled.Value)
            {
                loggerConfig.WriteTo.File(
                    file.Path,
                    rollingInterval: Day,
                    outputTemplate: "{Message}{NewLine}");
            }

            if (elasticSearch.Enabled != null && elasticSearch.Enabled.Value)
            {
                loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearch.Uri))
                {
                    AutoRegisterTemplate = true,
                });
            }

            if (tracing.Enabled != null && tracing.Enabled.Value)
            {
                loggerConfig.WriteTo.OpenTracing();
            }

            this.logger = loggerConfig.CreateLogger();
        }

        public void Write(LogLevel logLevel, string data) => this.logger.Write(Levels[logLevel], "{Json:l}", data);
    }
}
