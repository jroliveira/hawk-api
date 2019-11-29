namespace Hawk.Infrastructure.Logging.Methods
{
    using System;
    using System.Collections.Generic;

    using Hawk.Infrastructure.Logging.Configurations;

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

            if (config?.Sinks?.Console?.Enabled != null && config.Sinks.Console.Enabled.Value)
            {
                loggerConfig.WriteTo.Console(
                    outputTemplate: "{NewLine}{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ---{NewLine}{Message:lj}{NewLine}");
            }

            if (config?.Sinks?.File?.Enabled != null && config.Sinks.File.Enabled.Value)
            {
                loggerConfig.WriteTo.File(
                    config.Sinks.File.Path,
                    rollingInterval: Day,
                    outputTemplate: "{Message}{NewLine}");
            }

            if (config?.Sinks?.ElasticSearch?.Enabled != null && config.Sinks.ElasticSearch.Enabled.Value)
            {
                loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.Sinks.ElasticSearch.Uri))
                {
                    AutoRegisterTemplate = true,
                });
            }

            this.logger = loggerConfig.CreateLogger();
        }

        public void Write(LogLevel logLevel, string data) => this.logger.Write(Levels[logLevel], data);
    }
}
