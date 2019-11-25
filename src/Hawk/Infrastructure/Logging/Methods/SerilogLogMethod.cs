namespace Hawk.Infrastructure.Logging.Methods
{
    using System;

    using Hawk.Infrastructure.Logging.Configurations;

    using Serilog;
    using Serilog.Sinks.Elasticsearch;

    using static Serilog.RollingInterval;

    public sealed class SerilogLogMethod : ILogMethod
    {
        private readonly ILogger logger;

        public SerilogLogMethod(LogConfiguration config)
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console();

            if (config.Sinks.File.Enabled)
            {
                loggerConfig.WriteTo.File(config.Sinks.File.Path, rollingInterval: Day, outputTemplate: "{Message}{NewLine}");
            }

            if (config.Sinks.ElasticSearch.Enabled)
            {
                loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.Sinks.ElasticSearch.Uri))
                {
                    AutoRegisterTemplate = true,
                });
            }

            this.logger = loggerConfig.CreateLogger();
        }

        public void Write(string data) => this.logger.Information("{Json:l}", data);
    }
}
