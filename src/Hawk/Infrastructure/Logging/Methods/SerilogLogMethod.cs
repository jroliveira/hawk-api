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

        public SerilogLogMethod(LogConfiguration config) => this.logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(config.Sinks.File.Path, rollingInterval: Day, outputTemplate: "{Message}{NewLine}")
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config.Sinks.ElasticSearch.Uri))
            {
                AutoRegisterTemplate = true,
            })
            .CreateLogger();

        public void Write(string data) => this.logger.Information(data);
    }
}
