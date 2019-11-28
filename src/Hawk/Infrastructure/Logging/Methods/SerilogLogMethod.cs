namespace Hawk.Infrastructure.Logging.Methods
{
    using System;

    using Hawk.Infrastructure.Logging.Configurations;

    using Serilog;
    using Serilog.Formatting.Compact;
    using Serilog.Sinks.Elasticsearch;

    using static Serilog.Events.LogEventLevel;
    using static Serilog.RollingInterval;

    using ILogger = Serilog.ILogger;

    public sealed class SerilogLogMethod : ILogMethod
    {
        private readonly ILogger logger;

        public SerilogLogMethod(LogConfiguration config, Action<ILogger>? done = default)
        {
            var loggerConfig = new LoggerConfiguration();

            if (config?.Sinks?.Console?.Enabled != null && config.Sinks.Console.Enabled.Value)
            {
                loggerConfig
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore", Information)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(Information, "{Timestamp:HH:mm:ss}  {Level}  {CorrelationToken}  {Message}{NewLine}{Exception}");
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
            done?.Invoke(this.logger);
        }

        public void Write(string data) => this.logger.Information("{Json:l}", data);
    }
}
